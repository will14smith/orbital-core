using GraphQL.Types;
using Orbital.Schema.Clubs;
using Orbital.Schema.People;
using Orbital.Schema.Rounds;

namespace Orbital.Schema
{
    public class RootMutation : ObjectGraphType
    {
        public RootMutation()
        {
            ClubMutations.AddToRoot(this);
            PeopleMutation.AddToRoot(this);
            RoundsMutation.AddToRoot(this);
        }
    }
}