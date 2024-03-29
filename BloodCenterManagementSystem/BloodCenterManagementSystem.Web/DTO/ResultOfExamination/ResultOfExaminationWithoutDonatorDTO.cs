﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.DTO.ResultOfExamination
{
    public class ResultOfExaminationWithoutDonatorDTO
    {
        public double HB { get; set; }
        public double HT { get; set; }
        public double RBC { get; set; }
        public double WBC { get; set; }
        public double PLT { get; set; }
        public double MCH { get; set; }
        public double MCHC { get; set; }
        public double MCV { get; set; }
        public double NE { get; set; }
        public double EO { get; set; }
        public double BA { get; set; }
        public double LY { get; set; }
        public double MO { get; set; }
        public int BloodPressureUpper { get; set; }
        public int BloodPressureLower { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
    }
}
