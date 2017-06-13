using System.Collections.Generic;
using System.Linq.Expressions;

namespace Orbital.Data.Versioning
{
    internal class VersionEntityExpressionMapper : ExpressionVisitor
    {
        private readonly VersionEntityMapping _versionEntityMapping;
        private readonly IDictionary<ParameterExpression, ParameterExpression> _parameterMappings = new Dictionary<ParameterExpression, ParameterExpression>();

        public VersionEntityExpressionMapper(VersionEntityMapping versionEntityMapping)
        {
            _versionEntityMapping = versionEntityMapping;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var versionEntityMember = _versionEntityMapping.FindMapping(node.Member);
            if (versionEntityMember == null)
            {
                return base.VisitMember(node);
            }

            var expr = Visit(node.Expression);
            if (expr.Type == _versionEntityMapping.VersionEntityType)
            {
                return Expression.Property(expr, versionEntityMember);
            }

            return Expression.MakeMemberAccess(expr, node.Member);
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
                if (originalParameter.Type == _versionEntityMapping.EntityType)
                {
                    var newParameter = Expression.Parameter(_versionEntityMapping.VersionEntityType, originalParameter.Name);
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