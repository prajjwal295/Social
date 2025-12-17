using AutoMapper;
using Microsoft.AspNetCore.Mvc.Versioning;
using Social.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register Our Extensions
//typeof(Program) is just a convenient way to tell your extension
//which assembly to scan (since Program lives in the main assembly).
builder.RegisterServices(typeof(Program));

var app = builder.Build();

app.RegisterPipelineComponents(typeof(Program));

app.Run();
