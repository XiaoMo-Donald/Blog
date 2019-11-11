﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TzCA.Common.ViewModelComponents
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class PlainFacadeItemSpecification : Attribute
    {
        public string RelevanceId { get; set; }
        public PlainFacadeItemSpecification(string id) 
        {
            RelevanceId = id;
        }

    }
}
