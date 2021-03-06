﻿using Newtonsoft.Json;
using Permackathon.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Permackathon.BL.UseCases.Charts
{
    public partial class FusionChart
    {
        private readonly IUnitOfWork UnitOfWork;
        public enum Mois
        {
            Jan, Fev, Mar, Avr, Mai, Jui, Juil, Aoû, Sep, Oct, Nov, Déc
        }
        public enum Activities
        {
            EAT, GROW, LEARN
        }

        public FusionChart(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public string Fusion(int year)
        {
            Dictionary<string, List<KeyValuePair<string, double>>> dictionary = new Dictionary<string, List<KeyValuePair<string, double>>>();
            var activities = UnitOfWork.ActivityRepository.GetAll();
            foreach (var activity in activities)
            {
                //List<KeyValuePair<string, double>> dataValuePair = MonthValues(activity.Id, year);
                switch (activity.Id)
                {
                    case 1:
                        dictionary.Add(Activities.EAT.ToString(), MonthValues(activity.Id, year));
                        break;
                    case 2:
                        dictionary.Add(Activities.GROW.ToString(), MonthValues(activity.Id, year));
                        break;
                    case 3:
                        dictionary.Add(Activities.LEARN.ToString(), MonthValues(activity.Id, year));
                        break;
                    default:
                        break;
                }
            }

            var jsonData = JsonConvert.SerializeObject(dictionary);

            return jsonData;
        }

        private List<KeyValuePair<string, double>> MonthValues(int activityId, int year)
        {
            var dataValuePair = new List<KeyValuePair<string, double>>();

            dataValuePair.Add(new KeyValuePair<string, double>(Mois.Jan.ToString(), Math.Round((double)GetMonthSum(activityId, year, 1), 2)));
            dataValuePair.Add(new KeyValuePair<string, double>(Mois.Fev.ToString(), Math.Round((double)GetMonthSum(activityId, year, 2), 2)));
            dataValuePair.Add(new KeyValuePair<string, double>(Mois.Mar.ToString(), Math.Round((double)GetMonthSum(activityId, year, 3), 2)));
            dataValuePair.Add(new KeyValuePair<string, double>(Mois.Avr.ToString(), Math.Round((double)GetMonthSum(activityId, year, 4), 2)));
            dataValuePair.Add(new KeyValuePair<string, double>(Mois.Mai.ToString(), Math.Round((double)GetMonthSum(activityId, year, 5), 2)));
            dataValuePair.Add(new KeyValuePair<string, double>(Mois.Jui.ToString(), Math.Round((double)GetMonthSum(activityId, year, 6), 2)));
            dataValuePair.Add(new KeyValuePair<string, double>(Mois.Juil.ToString(), Math.Round((double)GetMonthSum(activityId, year, 7), 2)));
            dataValuePair.Add(new KeyValuePair<string, double>(Mois.Aoû.ToString(), Math.Round((double)GetMonthSum(activityId, year, 8), 2)));
            dataValuePair.Add(new KeyValuePair<string, double>(Mois.Sep.ToString(), Math.Round((double)GetMonthSum(activityId, year, 9), 2)));
            dataValuePair.Add(new KeyValuePair<string, double>(Mois.Oct.ToString(), Math.Round((double)GetMonthSum(activityId, year, 10), 2)));
            dataValuePair.Add(new KeyValuePair<string, double>(Mois.Nov.ToString(), Math.Round((double)GetMonthSum(activityId, year, 11), 2)));
            dataValuePair.Add(new KeyValuePair<string, double>(Mois.Déc.ToString(), Math.Round((double)GetMonthSum(activityId, year, 12), 2)));

            return dataValuePair;
        }

        private decimal GetMonthSum(int activityId, int year, int month)
        {
            return UnitOfWork.FinancialRepository
                             .GetAll()
                             .Where(f => f.Activity.Id == activityId && f.ReportDate.Year == year && f.ReportDate.Month == month).Sum(f => f.ActualSale);
        }
    }
}