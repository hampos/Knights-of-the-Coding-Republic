﻿using System.Collections.Generic;

namespace Ilida.Api.Models
{
    public class AccidentCar
    {
        public AccidentCar()
        {
            CarAccidentConditions = new List<CarAccidentCondition>();
        }

        public long Id { get; set; }

        public string CarPlate { get; set; }

        public string DamageText { get; set; }

        public string Remarks { get; set; }

        public long AccidentId { get; set; }
        public virtual Accident Accident { get; set; }

        public virtual ICollection<CarAccidentCondition> CarAccidentConditions { get; set; }
    }
}