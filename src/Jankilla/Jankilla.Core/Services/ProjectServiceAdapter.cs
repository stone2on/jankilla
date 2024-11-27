using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Jankilla.Core.Contracts;
using Jankilla.Core.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Services
{
    public class ProjectServiceAdapter : Proto.ProjectService.ProjectServiceBase
    {
        private Contracts.Project _project;

        public ProjectServiceAdapter(Contracts.Project project)
        {
            _project = project;
        }

        public override Task<Proto.Project> GetProject(Empty request, ServerCallContext context)
        {
            var projectResponse = new Proto.Project();

            foreach (var driver in _project.Drivers)
            {
                var driverResponse = new Proto.Driver()
                {
                    Id = driver.ID.ToString(),
                };
                foreach (var device in driver.Devices)
                {
                    var deviceResponse = new Proto.Device()
                    {
                        Id =  device.ID.ToString()
                    };
                    foreach (var block in device.Blocks)
                    {
                        var blockResponse = new Proto.Block()
                        {
                            Id = block.ID.ToString()
                        };
                        foreach (var tag in block.Tags)
                        {
                            var tagResponse = new Proto.Tag()
                            {
                                Id = tag.ID.ToString(),
                                Category = tag.Category,
                                Name = tag.Name,
                                Kind = (TagKind)tag.Discriminator,
                            };

                            blockResponse.Tags.Add(tagResponse);
                        }
                        deviceResponse.Blocks.Add(blockResponse);
                    }
                    driverResponse.Devices.Add(deviceResponse);
                }

                projectResponse.Drivers.Add(driverResponse);
            }


            return Task.FromResult(projectResponse);
        }

    }
}
