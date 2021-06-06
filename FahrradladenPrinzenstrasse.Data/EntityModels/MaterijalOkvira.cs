﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FahrradladenPrinzenstrasse.Data.EntityModels
{
   public class MaterijalOkvira
    {
        public int MaterijalOkviraId { get; set; }
        public string Naziv { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;
    }
}