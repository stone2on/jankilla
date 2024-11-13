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
        private Project _project;

        public ProjectServiceAdapter(Project project)
        {
            _project = project;
        }

        public override Task<ProjectResponse> GetProject(ProjectRequest request, ServerCallContext context)
        {
            var projectResponse = new ProjectResponse();
            foreach (var driver in _project.Drivers)
            {
                var driverResponse = new DriverResponse()
                {
                    Id = new GUID() { Value = driver.ID.ToString() }
                };
                foreach (var device in driver.Devices)
                {
                    var deviceResponse = new DeviceResponse()
                    {
                        Id = new GUID() { Value = device.ID.ToString() }
                    };
                    foreach (var block in device.Blocks)
                    {
                        var blockResponse = new BlockResponse()
                        {
                            Id = new GUID() { Value = block.ID.ToString() }
                        };
                        foreach (var tag in block.Tags)
                        {
                            var tagResponse = new TagResponse()
                            {
                                Id = new GUID() { Value = tag.ID.ToString() },
                                Category = tag.Category,
                                Name = tag.Name,
                                Type = tag.Discriminator.ToString(),
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
