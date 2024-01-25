using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model.LoginModel
{
    public class MenuItem
    {
        public int MenuId { get; set; }
        public int MenuLvl { get; set; }
        public string MenuTxt { get; set; }
        public string? MenuDesc { get; set; } = null;
        public string MenuUrl { get; set; }
        public int? MenuPrntId { get; set; }
        public int MenuIndx { get; set; }
        public string ImgUrl { get; set; } = null;
        public string HelpUrl { get; set; } = null;
        public string ToolTip { get; set; } = null;
        public string ShortKey { get; set; } = null;
        public bool DefaultAllow { get; set; } = false;
        public bool ActStatus { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; } = DateTime.Now;
        public int PrntCol { get; set; } = 0;
        public IEnumerable<MenuItem> TopLevelItems { get; set; }
        public List<MenuItem> Children { get; set; } = new List<MenuItem>();
    }
}
