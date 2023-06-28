using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftForms.Model.Entities
{
    public class Page
    {
        public Guid Id { get; set; }
        public List<Question>? Questions { get; set; }
    }
}
