using System.Collections.Generic;
using System.Linq.Expressions;

namespace Orbital.Data.Versioning
{
    internal class VersionExpressionMapper : ExpressionVisitor
    {
        private readonly VersionModel _versionModel;
        private readonly IDictionary<ParameterExpression, ParameterExpression> _parameterMappings = new Dictionary<ParameterExpression, ParameterExpression>();

        public VersionExpressionMapper(VersionModel versionModel)
        {
            _versionModel = versionModel;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var expr = Visit(node.Expression);
            if (expr.Type != _versionModel.VersionType)
            {
                return Expression.MakeMemberAccess(expr, node.Member);
            }

            if (_versionModel.TryFindMapping(node.Member, out var versionMember))
            {
                return Expression.Property(expr, versionMember);
            }

            return base.VisitMember(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_parameterMappings.TryGetValue(node, out var mappedParameter))
            {
                return mappedParameter;
            }

            return base.VisitParameter(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var parameters = new List<ParameterExpression>();
            foreach (var originalParameter in node.Parameters)
            {
                if (originalParameter.Type == _versionModel.EntityType)
                {
                    var newParameter = Expression.Parameter(_versionModel.VersionType, originalParameter.Name);

                    _parameterMappings.Add(originalParameter, newParameter);
                    parameters.Add(newParameter);
                }
                else
                {
                    parameters.Add(originalParameter);
                }
            }

            var body = Visit(node.Body);

            return Expression.Lambda(body, node.TailCall, parameters);
        }
    }
}