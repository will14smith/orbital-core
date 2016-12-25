using System;
using GraphQL.Types;

namespace Orbital.Schema
{
  public class RootSchema : GraphQL.Types.Schema
  {
    public RootSchema(Func<Type, GraphType> resolveType)
      : base(resolveType)
    {
      Query = (RootQuery)resolveType(typeof(RootQuery));
      Mutation = (RootMutation)resolveType(typeof(RootMutation));
    }
  }
}