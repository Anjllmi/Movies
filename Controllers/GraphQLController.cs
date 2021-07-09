using GraphQL;
using GraphQL.Types;
using GraphQL.SystemTextJson;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenIddict.Validation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movies.Data;
using Movies.GraphQLArchives;

using Movies.Models;
using Movies.Models.ViewModels;
using Movies.Services;
using Movies.Utilities;
using System.Text;
using System.Threading;
using System.Text.Json;
using GraphQL.Introspection;

namespace Movies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class GraphQLController : ControllerBase
    {
        //private readonly GraphQLSingleton _GraphQLSingleton;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private readonly IDocumentExecuter _executer;
        private readonly IDocumentWriter _documentWriter;
        private readonly ISchema _schema;

        public GraphQLController(ILogger<GraphQLController> logger, ApplicationDbContext context, IDocumentExecuter executer, ISchema schema, IDocumentWriter documentWriter)
        {
            _context = context;
            _logger = logger;
            _executer = executer;
            _schema = schema;
            _documentWriter = documentWriter;
        }

        // POST: api/GraphQL
        [HttpPost]
        [AllowAnonymous]
        public async Task PostGraphQL(GraphQLViewModel model)
        {
            var result = await _executer.ExecuteAsync(options =>
            {
                options.Schema = _schema;
                options.Query = model.Query;
                options.Inputs = null;
                options.EnableMetrics = true;
                options.UnhandledExceptionDelegate = ctx => Console.WriteLine("error: " + ctx.OriginalException.Message);
                options.CancellationToken = CancellationToken.None;
            });
            await WriteResponseAsync(HttpContext, result, CancellationToken.None);
            //Console.WriteLine($"response ===================  { result.Data}");           
        }

        // POST: api/GraphQL/v1
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> V1(GraphQLViewModel model)
        {
            var json = await _schema.ExecuteAsync(_ =>
            {
                _.Query = model.Query;
                _.UnhandledExceptionDelegate = ctx => Console.WriteLine("error: " + ctx.OriginalException.Message);
            });
            return Content(json, "application/json", contentEncoding: Encoding.UTF8);
        }

        private async Task WriteResponseAsync(HttpContext context, ExecutionResult result, CancellationToken cancellationToken)
        {

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200; // OK

            await _documentWriter.WriteAsync(context.Response.Body, result, cancellationToken);
        }
    }
}