using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.GraphQLArchives.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//using GraphQL.Types;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using WebApplication1.Data;
//using WebApplication1.Graphql.Models;
//using WebApplication1.Models;

namespace Movies.GraphQLArchives
{
    public class CustomQuery : ObjectGraphType
    {
        public CustomQuery(ApplicationDbContext dbContext)
        {
            Name = "Query";

            Field<MovieType>(
              "movie",
              resolve: context => dbContext.Movies
                  .Include(x => x.Category)
                  .FirstOrDefault()
            );

            Field<ListGraphType<MovieType>>(
             "movies",
             resolve: context => dbContext.Movies.ToList()
           );

            Field<CategoryType>(
                "category",
                resolve: context => dbContext.Categories.FirstOrDefault()
            );

            Field<ListGraphType<CategoryType>>(
               "categories",
               resolve: context => dbContext.Categories.ToList()
           );
        }
    }
}
