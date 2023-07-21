using Microsoft.AspNetCore.Mvc;
using peopleApi.Models;
using System.Text.Json;
using System.Xml.Linq;

namespace peopleApi.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class PeopleController : ControllerBase
    {
        private string filePath = "data/dataXml.xml";
        private List<PeopleModel> data = new List<PeopleModel>();

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PeopleModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                var personas = doc.Root.Elements("Person").Select(personElement => new
                {
                    id = (int)personElement.Element("id"),
                    name = (string)personElement.Element("name"),
                    lastName = (string)personElement.Element("lastName"),
                    age = (int)personElement.Element("age")
                }).ToList();

                foreach (var persona in personas)
                {
                    data.Add(new PeopleModel()
                    {
                        Id = persona.id,
                        Name = persona.name,
                        LastName = persona.lastName,
                        Age = persona.age,
                    });
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                return NotFound();
            }

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PeopleModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            return Ok("");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PeopleModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Post([FromBody] PeopleModel model)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement newPersonElement = new XElement("Person",
                    new XElement("id", model.Id),
                    new XElement("name", model.Name),
                    new XElement("lastName", model.LastName),
                    new XElement("age", model.Age));

                doc.Root.Add(newPersonElement);

                doc.Save(filePath);

                data.Add(model);

                return Created("", model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Error al agregar el registro al archivo XML",
                    Detail = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PeopleModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Put([FromRoute] long id, [FromBody] PeopleModel model)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement personElement = doc.Root.Elements("Person")
                    .FirstOrDefault(e => (int)e.Element("id") == id);

                if (personElement == null)
                {
                    return NotFound();
                }

                personElement.Element("name").Value = model.Name;
                personElement.Element("lastName").Value = model.LastName;
                personElement.Element("age").Value = model.Age.ToString();

                doc.Save(filePath);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Error al actualizar el registro en el archivo XML",
                    Detail = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement personElement = doc.Root.Elements("Person")
                    .FirstOrDefault(e => (int)e.Element("id") == id);

                if (personElement == null)
                {
                    return NotFound();
                }

                personElement.Remove();

                doc.Save(filePath);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Error al eliminar el registro del archivo XML",
                    Detail = ex.Message
                });
            }
        }
    }
}