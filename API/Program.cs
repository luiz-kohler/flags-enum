using API;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<Context>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
app.MapOpenApi();

app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "User API V1"));

app.UseHttpsRedirection();


// CRUD USERS
app.MapGet("/api/users", async (
    Context context,
    bool? isHirer,
    bool? isPassenger,
    bool? isFinancialManager) =>
{
    var query = context.Users.AsQueryable();

    if (isHirer.HasValue)
    {
        if (isHirer.Value)
            query = query.Where(u => (u.Roles & ERoles.Hirer) == ERoles.Hirer);
        else
            query = query.Where(u => (u.Roles & ERoles.Hirer) != ERoles.Hirer);
    }

    if (isPassenger.HasValue)
    {
        if (isPassenger.Value)
            query = query.Where(u => (u.Roles & ERoles.Passenger) == ERoles.Passenger);
        else
            query = query.Where(u => (u.Roles & ERoles.Passenger) != ERoles.Passenger);
    }

    if (isFinancialManager.HasValue)
    {
        if (isFinancialManager.Value)
            query = query.Where(u => (u.Roles & ERoles.FinancialManager) == ERoles.FinancialManager);
        else
            query = query.Where(u => (u.Roles & ERoles.FinancialManager) != ERoles.FinancialManager);
    }

    var users = await query.ToListAsync();
    var userDtos = users.Select(Helpers.ConvertToUserDto).ToList();

    return Results.Ok(userDtos);
});

app.MapGet("/api/users/{id}", async (int id, Context context) =>
{
    var user = await context.Users.FindAsync(id);
    return user != null ? Results.Ok(Helpers.ConvertToUserDto(user)) : Results.NotFound();
});

app.MapPost("/api/users", async (CreateUserDto userDto, Context context) =>
{
    var user = new User
    {
        Name = userDto.Name,
        Roles = Helpers.ConvertToRoles(userDto.IsHirer, userDto.IsPassenger, userDto.IsFinancialManager)
    };

    context.Users.Add(user);
    await context.SaveChangesAsync();
    return Results.Created($"/api/users/{user.Id}", Helpers.ConvertToUserDto(user));
});

app.MapPut("/api/users/{id}", async (int id, UpdateUserDto updatedUserDto, Context context) =>
{
    var user = await context.Users.FindAsync(id);
    if (user == null) return Results.NotFound();

    user.Name = updatedUserDto.Name;
    user.Roles = Helpers.ConvertToRoles(updatedUserDto.IsHirer, updatedUserDto.IsPassenger, updatedUserDto.IsFinancialManager);

    await context.SaveChangesAsync();
    return Results.Ok(Helpers.ConvertToUserDto(user));
});

app.MapDelete("/api/users/{id}", async (int id, Context context) =>
{
    var user = await context.Users.FindAsync(id);
    if (user == null) return Results.NotFound();

    context.Users.Remove(user);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/users/detailed", async (Context context) =>
{
    var users = await context.Users.ToListAsync();
    var detailedUsers = users.Select(user => new
    {
        Id = user.Id,
        Name = user.Name,
        RolesValue = user.Roles,
        IsHirer = user.Roles.HasFlag(ERoles.Hirer),
        IsPassenger = user.Roles.HasFlag(ERoles.Passenger),
        IsFinancialManager = user.Roles.HasFlag(ERoles.FinancialManager),
        Roles = Helpers.GetRoleDescriptions(user.Roles)
    }).ToList();

    return Results.Ok(detailedUsers);
});

// ADD SPECIFIC ROLES 
app.MapPut("/users/{id}/add-hirer-role", async (int id, Context context) =>
{
    var user = await context.Users.FindAsync(id);
    if (user == null) return Results.NotFound();

    user.Roles |= ERoles.Hirer;
    await context.SaveChangesAsync();

    return Results.Ok(new { message = "Hirer role added successfully", user = Helpers.ConvertToUserDto(user) });
});

app.MapPut("/users/{id}/add-passenger-role", async (int id, Context context) =>
{
    var user = await context.Users.FindAsync(id);
    if (user == null) return Results.NotFound();
      
    user.Roles |= ERoles.Passenger;
    await context.SaveChangesAsync();

    return Results.Ok(new { message = "Passenger role added successfully", user = Helpers.ConvertToUserDto(user) });
});

app.MapPut("/users/{id}/add-financial-manager-role", async (int id, Context context) =>
{
    var user = await context.Users.FindAsync(id);
    if (user == null) return Results.NotFound();

    user.Roles |= ERoles.FinancialManager;
    await context.SaveChangesAsync();

    return Results.Ok(new { message = "Financial manager role added successfully", user = Helpers.ConvertToUserDto(user) });
});

// REMOVE SPECIFIC ROLES 
app.MapPut("/users/{id}/remove-hirer-role", async (int id, Context context) =>
{
    var user = await context.Users.FindAsync(id);
    if (user == null) return Results.NotFound();

    user.Roles &= ~ERoles.Hirer;
    await context.SaveChangesAsync();

    return Results.Ok(new { message = "Hirer role removed successfully", user = Helpers.ConvertToUserDto(user) });
});

app.MapPut("/users/{id}/remove-passenger-role", async (int id, Context context) =>
{
    var user = await context.Users.FindAsync(id);
    if (user == null) return Results.NotFound();

    user.Roles &= ~ERoles.Passenger;
    await context.SaveChangesAsync();

    return Results.Ok(new { message = "Passenger role removed successfully", user = Helpers.ConvertToUserDto(user) });
});

app.MapPut("/users/{id}/remove-financial-manager-role", async (int id, Context context) =>
{
    var user = await context.Users.FindAsync(id);
    if (user == null) return Results.NotFound();

    user.Roles &= ~ERoles.FinancialManager;
    await context.SaveChangesAsync();

    return Results.Ok(new { message = "Financial manager role removed successfully", user = Helpers.ConvertToUserDto(user) });
});

// ADD OR REMOVE ALL ROLES 
app.MapPut("/users/{id}/add-all-roles", async (int id, Context context) =>
{
    var user = await context.Users.FindAsync(id);
    if (user == null) return Results.NotFound();

    user.Roles = ERoles.Hirer | ERoles.Passenger | ERoles.FinancialManager;
    await context.SaveChangesAsync();

    return Results.Ok(new { message = "All roles added successfully", user = Helpers.ConvertToUserDto(user) });
});

app.MapPut("/users/{id}/remove-all-roles", async (int id, Context context) =>
{
    var user = await context.Users.FindAsync(id);
    if (user == null) return Results.NotFound();

    user.Roles = ERoles.None;
    await context.SaveChangesAsync();

    return Results.Ok(new { message = "All roles removed successfully", user = Helpers.ConvertToUserDto(user) });
});

app.Run();