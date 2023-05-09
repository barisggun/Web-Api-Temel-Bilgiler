using _01_EFCore.Entities;
using _01_EFCore.Models;
using Azure.Core;
using MFramework.Services.FakeData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace _01_EFCore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private TodoDbContext _db;
        public TodoController(TodoDbContext db)
        {
            _db = db;
        }



        [HttpGet("generate-fakedata")]
        public IActionResult GenerateFakeData()
        {
            if (_db.Todos.Any())
                return Ok("Geçici veriler zaten oluşturulmuş.");

            for (int i = 1; i < 21; i++)
            {
                _db.Todos.Add(new Todo
                {
                    Header = TextData.GetSentence(),
                    IsCompleted = BooleanData.GetBoolean(),
                    Description = TextData.GetSentences(2)
                });
            }

            _db.SaveChanges();
            return Ok("Ok");
        }



        [HttpGet("list")]
        //[ProducesResponseType((int)HttpStatusCode.OK, Type=typeof(List<Todo>))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<TodoResponse>))]
        public IActionResult List()
        {
            //return Ok(_db.Todos.ToList());


            //1.Map Etme Yontemi
            //List<Todo> list = _db.Todos.ToList();
            //List<TodoResponse> result = new List<TodoResponse>();
            //foreach (Todo item in list)
            //{
            //    result.Add(new TodoResponse
            //    {
            //        Id = item.Id,
            //        Description=item.Description,   
            //        Header = item.Header,
            //        IsCompleted = item.IsCompleted
            //    });
            //}
            //return Ok(result);  


            //2.Map Etme Yontemi (Foreach Free)
            List<TodoResponse> result = _db.Todos.Select(t => new TodoResponse
            {
                Description = t.Description,
                Id = t.Id,
                Header = t.Header,
                IsCompleted = t.IsCompleted,

            }).ToList();

            return Ok(result);

            //AUTOMAPPER arastirilacak.
        }



        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<TodoResponse>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public IActionResult Create([FromBody] TodoCreateModel model)
        {

            Todo todo = new Todo
            {
                Description = model.Description,
                Header = model.Header,
                IsCompleted = false
            };

            _db.Todos.Add(todo);
            int affectedRows = _db.SaveChanges();
            if (affectedRows > 0)
            {
                TodoResponse result = new TodoResponse
                {
                    Id = todo.Id,
                    Header = todo.Header,
                    IsCompleted = todo.IsCompleted,
                    Description = todo.Description,
                };
                return Created(string.Empty, result);
            }
            else
            {
                return BadRequest("Kayıt yapılamadı.");
            }
        }



        [HttpPut("edit/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TodoResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        public IActionResult Update([FromRoute] int id, [FromBody] TodoUpdateModel model)
        {
            Todo todo = _db.Todos.Find(id);

            if (todo == null)
            {
                return NotFound("Kayit bulunamadi.");
            }
            todo.Description = model.Description;
            todo.Header = model.Header;
            todo.IsCompleted = model.IsCompleted;

            int affectedRows = _db.SaveChanges();
            if (affectedRows > 0)
            {
                TodoResponse result = new TodoResponse
                {
                    Id = todo.Id,
                    Description = todo.Description,
                    Header = todo.Header,
                    IsCompleted = todo.IsCompleted,
                };
                return Ok(result);
            }
            else
            {
                return BadRequest("Guncelleme yapilamadi.");
            }



        }




        [HttpGet("getbyid/{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            Todo todo = _db.Todos.Find(id);

            if (todo == null)
                return NotFound("Kayıt bulunamadı");

            TodoResponse response = new TodoResponse
            {
                Id = todo.Id,
                Description = todo.Description,
                Header = todo.Header,
                IsCompleted = todo.IsCompleted,
            };
            return Ok(response);
        }




        [HttpPatch("change-state/{id}/{isCompleted}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TodoResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        public IActionResult UpdateIsCompleted([FromRoute] int id, [FromRoute] bool isCompleted)
        {
            Todo todo = _db.Todos.Find(id);

            if (todo == null)
                return NotFound("Kayıt bulunamadi");

            todo.IsCompleted = isCompleted;
            int affectedRows = _db.SaveChanges();
            if (affectedRows > 0)
            {
                TodoResponse todoResponse = new TodoResponse
                {
                    Id = todo.Id,
                    Description = todo.Description,
                    Header = todo.Header,
                    IsCompleted = todo.IsCompleted
                };

                return Ok(todoResponse);
            }
            else
                return BadRequest("Kayit guncellenemedi.");
        }




        [HttpDelete("delete/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        public IActionResult Delete([FromRoute] int id)
        {
            Todo todo = _db.Todos.Find(id);

            if (todo == null) 
                return NotFound("Kayıt bulunamadı");

            _db.Todos.Remove(todo);
            int affectedRows = _db.SaveChanges();
            if (affectedRows > 0) 
                return Ok("Kayıt silindi");
            else
                return BadRequest("Kayıt silinemedi");
        }




        [HttpDelete("delete-all")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        public IActionResult DeleteAll([FromRoute] int id)
        {
            List<Todo> todos = _db.Todos.ToList();

            if (todos == null)
                return NotFound("Kayıt bulunamadı");

            foreach (var item in todos)
            {
                _db.Todos.Remove(item);
            }
           
            int affectedRows = _db.SaveChanges();
            if (affectedRows > 0)
                return Ok("Kayıtlar silindi");
            else
                return BadRequest("Kayıtlar silinemedi");
        }



    }

}

