﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Mobile
{

    public class MasterDetailPageMasterMenuItem
    {
        public MasterDetailPageMasterMenuItem()
        {
            TargetType = typeof(MasterDetailPageMasterMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}