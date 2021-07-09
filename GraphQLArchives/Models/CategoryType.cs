using GraphQL.Types;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.GraphQLArchives.Models
{
    public class CategoryType : ObjectGraphType<Category>
    {
        public CategoryType()
        {
            Field(x => x.Id).Description("The Id of the Category.");
            Field(x => x.Name).Description("The name of the Category.");
        }
    }
}
