using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using restapi.Models;

namespace restapi.Controllers {
    public class RootController : Controller {
        // GET api/values
        [Route("~/")]
        [HttpGet]
        [Produces(ContentTypes.Root)]
        [ProducesResponseType(typeof(IDictionary<ApplicationRelationship, DocumentLink>), 200)]
        public IDictionary<ApplicationRelationship, List<DocumentLink>> Get() {
            return new Dictionary<ApplicationRelationship, List<DocumentLink>>() {  { 
                    
                    ApplicationRelationship.ImageProcess, new List<DocumentLink>() { 
                        new DocumentLink() {
                            Method = Method.Post,
                            Type = ContentTypes.ImageProcessor,
                            Reference = "Image/{path}"
                        },

                        new DocumentLink() {
                            Method = Method.Post,
                            Type = ContentTypes.ImageProcessor,
                            Reference = "Image/picturebox/thumbailcmd"
                        },

                        new DocumentLink() {
                            Method = Method.Post,
                            Type = ContentTypes.ImageProcessor,
                            Reference = "Image/picturebox/rotationcmd/{degrees}"
                        },

                        new DocumentLink() {
                            Method = Method.Post,
                            Type = ContentTypes.ImageProcessor,
                            Reference = "Image/picturebox/flippedcmd/{direction}"
                        },
                        
                        new DocumentLink() {
                            Method = Method.Post,
                            Type = ContentTypes.ImageProcessor,
                            Reference = "Image/picturebox/sizecmd/<{width},{height}>"
                        },
                        new DocumentLink() {
                            Method = Method.Get,
                            Type = ContentTypes.ImageProcessor,
                            Reference = "Image/picturebox"
                        }
                    }
                }
            };
        }
    }
}
