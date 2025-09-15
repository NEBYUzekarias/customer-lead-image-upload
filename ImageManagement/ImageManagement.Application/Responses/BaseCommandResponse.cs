using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManagement.Application.Responses
{
    public class BaseCommandResponse
    {
        public int Id { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
    }
}
