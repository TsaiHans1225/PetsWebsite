﻿namespace PetsWebsite.Models.ViewModels
{
    public class AfterEditClinicViewModel
    {
        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string? Describe { get; set; }
        public string Emergency { get; set; }
        public string? Service { get; set; }
        public string? ClinicMap { get; set; }
    }
}
