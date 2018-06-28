﻿// Sait ORHAN -- 08.11.2014 -> HealthMonitoringSystem -- HealthMonitoringSystem.DAL -- EfPrescriptionDAL.cs

#region usings

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HealthMonitoringSystem.DAL.Abstract;
using HealthMonitoringSystem.Entity;

#endregion

namespace HealthMonitoringSystem.DAL.Content.MsSqlContent
{
    public class MsSqlPrescriptionDal : IPrescriptionDAL
    {
        public Prescription Select(int id)
        {
            using (MsSqlHealthContext ctx = new MsSqlHealthContext())
            {
                return
                    ctx.Prescriptions.Include("Examination")
                        .Include("PrescriptionItems")
                        .FirstOrDefault(d => d.Id == id);
            }
        }

        public List<Prescription> Prescriptions(bool? isActive = true)
        {
            using (MsSqlHealthContext ctx = new MsSqlHealthContext())
            {
                return
                    ctx.Prescriptions.Where(c => isActive == null || c.IsActive == isActive)
                        .Include("PrescriptionItems")
                        .ToList();
            }
        }

        public bool Insert(Prescription newPrescription)
        {
            using (MsSqlHealthContext ctx = new MsSqlHealthContext())
            {
                ctx.Prescriptions.Add(newPrescription);
                return ctx.SaveChanges() > -1;
            }
        }

        public bool Update(Prescription newInfoPresciption)
        {
            using (MsSqlHealthContext ctx = new MsSqlHealthContext())
            {
                Prescription prescription = ctx.Prescriptions.FirstOrDefault(d => d.Id == newInfoPresciption.Id);

                if (prescription == null)
                {
                    return false;
                }

                prescription.ExaminationId = newInfoPresciption.ExaminationId;
                prescription.DoctorNote = newInfoPresciption.DoctorNote;
                prescription.IsActive = newInfoPresciption.IsActive;

                return ctx.SaveChanges() > -1;
            }
        }

        public bool Delete(int id)
        {
            using (MsSqlHealthContext ctx = new MsSqlHealthContext())
            {
                Prescription prescription = ctx.Prescriptions.FirstOrDefault(d => d.Id == id);
                if (prescription == null)
                {
                    return false;
                }
                ctx.Prescriptions.Remove(prescription);
                return ctx.SaveChanges() > -1;
            }
        }
    }
}