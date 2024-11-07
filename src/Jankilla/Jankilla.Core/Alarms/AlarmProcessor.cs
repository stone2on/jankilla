using Jankilla.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Alarms
{
    internal class AlarmProcessor
    {
        private readonly Project _project;

        public AlarmProcessor(Project project)
        {
            this._project = project;
        }

        public void ProcessAlarms()
        {
            var stack = new Stack<BaseAlarm>(_project.Alarms);

            while (stack.Count > 0)
            {
                var alarm = stack.Pop();

                switch (alarm)
                {
                    case ComplexAlarm complexAlarm:
                        foreach (var subAlarm in complexAlarm.SubAlarms)
                        {
                            stack.Push(subAlarm);
                        }
                        break;

                    case TagAlarm tagAlarm:
                        if (tagAlarm.TagID != Guid.Empty)
                        {
                            var tag = _project.FindTagOrNull(tagAlarm.TagID);
                            if (tag != null)
                            {
                                tagAlarm.SetTag(tag);
                            }
                        }
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }
        }
    }
}
