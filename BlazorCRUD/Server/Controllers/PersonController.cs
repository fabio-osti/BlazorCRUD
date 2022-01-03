using BlazorCRUD.Shared;
using BlazorCRUD.Shared.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.Json;

namespace BlazorCRUD.Server.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class PersonController : Controller
	{
		private readonly PersonContext _context;

		public PersonController(PersonContext context)
		{
			_context = context;
		}

		public IActionResult Create(Person person)
		{
			try {
				var p = _context.Persons!.Add(person);
				_context.SaveChanges();
				return Ok((int)p.Entity.Id!);
			} catch (DbUpdateException e) {
				return Problem(e.Message);
			}
		}

		public IActionResult Read(
			int rows, int page, PersonOrdering orderBy, string? name, short? age, bool? sex, HairColor? hc
		)
		{
			try {
				IQueryable<Person> persons = _context.Persons!;
				if (name != null)
					persons = persons.Where(person => person.Name == name);
				if (age != null)
					persons = persons.Where(person => person.Age == age);
				if (sex != null)
					persons = persons.Where(person => person.Sex == sex);
				if (hc != null)
					persons = persons.Where(person => person.HairColor == hc);

				persons = orderBy switch
				{
					PersonOrdering.NameA => persons.OrderBy(Person => Person.Name),
					PersonOrdering.NameD => persons.OrderByDescending(Person => Person.Name),
					PersonOrdering.AgeA => persons.OrderBy(Person => Person.Age),
					PersonOrdering.AgeD => persons.OrderByDescending(Person => Person.Age),
					PersonOrdering.SexA => persons.OrderBy(Person => Person.Sex),
					PersonOrdering.SexD => persons.OrderByDescending(Person => Person.Sex),
					PersonOrdering.HairColorA => persons.OrderBy(Person => Person.HairColor),
					PersonOrdering.HairColorD => persons.OrderByDescending(Person => Person.HairColor),
					_ => persons,
				};

				return Ok(new ApiResponse<Person>(
					persons.Skip(rows * (page - 1)).Take(rows).ToArray(), persons.Count()));
			} catch (Exception e) {
				return Problem(e.Message);
			}
		}

		public IActionResult Update(Person person)
		{
			try {
				_context.Persons!.Update(person);
				_context.SaveChanges();
				return Ok();
			} catch (DbUpdateException e) {
				return Problem(e.Message);
			}
		}

		public IActionResult Delete(int id)
		{
			try {
				var remove = _context.Persons!.Find(id);
				if (remove == null)
					return Problem("Id not found");
				_context.Remove(remove);
				_context.SaveChanges();
				return Ok();
			} catch (DbUpdateException e) {
				return Problem(e.Message);
			}
		}
	}
}
