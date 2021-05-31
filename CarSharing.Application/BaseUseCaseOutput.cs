using CarSharing.Core.Enums;
using System.Collections.Generic;

namespace CarSharing.Application
{
    public abstract class BaseUseCaseOutput
    {
        public List<Error> Errors { get; set; } = new List<Error>();
    }
}
