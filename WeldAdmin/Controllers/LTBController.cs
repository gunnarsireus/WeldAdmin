using Microsoft.VisualBasic;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using LTBCore;
using SireusRR.Models;

namespace SireusRR.Controllers
{
    public class LtbController : Controller
    {
        private static readonly int[] InstalledBasePerYear = new int[LtbCommon.MaxYear + 1];
        private static readonly double[] FailureRatePerYear = new double[LtbCommon.MaxYear + 1];
        private static readonly double[] RepairLossPerYear = new double[LtbCommon.MaxYear + 1];
        private static readonly int[] RegionalStocksPerYear = new int[LtbCommon.MaxYear + 1];

        private static string _stock = string.Empty;
        private static string _safety = string.Empty;
        private static string _failed = string.Empty;
        private static string _repaired = string.Empty;
        private static string _lost = string.Empty;
        private static string _total = string.Empty;
        private static MemoryStream _ltbChart;

        private static double _confidenceLevelConverted;
        private static int _leadDays;
        private static double _confidenceLevelFromNormsInv;
        private static bool _repairSelected;
        private static bool _mtbfSelected;
        // GET: /LTB/

        private static DateTime LifeTimeBuy { get; set; }
        private static DateTime EOSDate { get; set; }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        private FileContentResult ClearedChart
        {
            get
            {
                var ms = LtbCommon.GetEmptyChart(800, 14);
                return File(ms.GetBuffer(), "image/png");
            }
        }


        private static int GetServiceDays(string LTBDate, string EOSDate)
        {
            DateTime lifeTimeBuy = Convert.ToDateTime(LTBDate);
            DateTime endOfService = Convert.ToDateTime(EOSDate);
            return Convert.ToInt32(DateTimeUtil.DateDiff(DateTimeUtil.DateInterval.Day, lifeTimeBuy, endOfService));
        }


        private static int GetServiceYears(string LTBDate, string EOSDate)
        {
            DateTime lifeTimeBuy = Convert.ToDateTime(LTBDate);
            DateTime endOfService = Convert.ToDateTime(EOSDate);
            if (lifeTimeBuy.Year == endOfService.Year)
            {
                return 0;
            }

            var newYear = Convert.ToDateTime(lifeTimeBuy.Year + "-01-01");
            if (IsLeapYear(lifeTimeBuy.Year) & DateTimeUtil.DateDiff(DateTimeUtil.DateInterval.Day, newYear, lifeTimeBuy) < 59)
            {
                return Convert.ToInt32((DateTimeUtil.DateDiff(DateTimeUtil.DateInterval.Day, lifeTimeBuy, endOfService) + CountLeaps(lifeTimeBuy.Year) - CountLeaps(endOfService.Year) - 2) / 365);
            }
            return Convert.ToInt32((DateTimeUtil.DateDiff(DateTimeUtil.DateInterval.Day, lifeTimeBuy, endOfService) + CountLeaps(lifeTimeBuy.Year) - CountLeaps(endOfService.Year) - 1) / 365);
        }

        private void ConvertInput(int finalYear, ref double confidenceLevelFromNormsInv, ref double confidenceLevelConverted, string confidence, string repairLeadTime, bool repairPossible, bool mtbfSelected, string LTBDate, string EOSDate,
                         string IB0, string IB1, string IB2, string IB3, string IB4, string IB5, string IB6, string IB7, string IB8, string IB9, string IB10,
                         string FR0, string FR1, string FR2, string FR3, string FR4, string FR5, string FR6, string FR7, string FR8, string FR9,
                         string RS0, string RS1, string RS2, string RS3, string RS4, string RS5, string RS6, string RS7, string RS8, string RS9,
                         string RL0, string RL1, string RL2, string RL3, string RL4, string RL5, string RL6, string RL7, string RL8, string RL9)
        {
            switch (confidence)
            {
                //Confidence Level

                case "60":
                    confidenceLevelFromNormsInv = LtbCommon.GetConfidenceLevelFromNormsInv(0.6);
                    confidenceLevelConverted = 0.6;

                    break;
                case "70":
                    confidenceLevelFromNormsInv = LtbCommon.GetConfidenceLevelFromNormsInv(0.7);
                    confidenceLevelConverted = 0.7;

                    break;
                case "80":
                    confidenceLevelFromNormsInv = LtbCommon.GetConfidenceLevelFromNormsInv(0.8);
                    confidenceLevelConverted = 0.8;

                    break;
                case "90":
                    confidenceLevelFromNormsInv = LtbCommon.GetConfidenceLevelFromNormsInv(0.9);
                    confidenceLevelConverted = 0.9;

                    break;
                case "95":
                    confidenceLevelFromNormsInv = LtbCommon.GetConfidenceLevelFromNormsInv(0.95);
                    confidenceLevelConverted = 0.95;

                    break;
                case "995":
                    confidenceLevelFromNormsInv = LtbCommon.GetConfidenceLevelFromNormsInv(0.995);
                    confidenceLevelConverted = 0.995;

                    break;
            }

            var yearCnt = 0;
            var ci = new CultureInfo("sv-SE");
            while (yearCnt <= finalYear)
            {
                switch (yearCnt)
                {
                    case 0:
                        InstalledBasePerYear[0] = int.Parse(IB0);
                        RegionalStocksPerYear[0] = int.Parse(RS0);
                        FailureRatePerYear[0] = ConvertMtbfToFr(Convert.ToDouble(FR0, ci));
                        RepairLossPerYear[0] = Convert.ToDouble(Convert.ToDouble(RL0, ci) / 100);

                        break;
                    case 1:
                        InstalledBasePerYear[1] = int.Parse(IB1);
                        RegionalStocksPerYear[1] = int.Parse(RS1);
                        FailureRatePerYear[1] = ConvertMtbfToFr(Convert.ToDouble(FR1, ci));
                        RepairLossPerYear[1] = Convert.ToDouble(Convert.ToDouble(RL1, ci) / 100);

                        break;
                    case 2:
                        InstalledBasePerYear[2] = int.Parse(IB2);
                        RegionalStocksPerYear[2] = int.Parse(RS2);
                        FailureRatePerYear[2] = ConvertMtbfToFr(Convert.ToDouble(FR2, ci));
                        RepairLossPerYear[2] = Convert.ToDouble(Convert.ToDouble(RL2, ci) / 100);

                        break;
                    case 3:
                        InstalledBasePerYear[3] = int.Parse(IB3);
                        RegionalStocksPerYear[3] = int.Parse(RS3);
                        FailureRatePerYear[3] = ConvertMtbfToFr(Convert.ToDouble(FR3, ci));
                        RepairLossPerYear[3] = Convert.ToDouble(Convert.ToDouble(RL3, ci) / 100);

                        break;
                    case 4:
                        InstalledBasePerYear[4] = int.Parse(IB4);
                        RegionalStocksPerYear[4] = int.Parse(RS4);
                        FailureRatePerYear[4] = ConvertMtbfToFr(Convert.ToDouble(FR4, ci));
                        RepairLossPerYear[4] = Convert.ToDouble(Convert.ToDouble(RL4, ci) / 100);

                        break;
                    case 5:
                        InstalledBasePerYear[5] = int.Parse(IB5);
                        RegionalStocksPerYear[5] = int.Parse(RS5);
                        FailureRatePerYear[5] = ConvertMtbfToFr(Convert.ToDouble(FR5, ci));
                        RepairLossPerYear[5] = Convert.ToDouble(Convert.ToDouble(RL5, ci) / 100);

                        break;
                    case 6:
                        InstalledBasePerYear[6] = int.Parse(IB6);
                        RegionalStocksPerYear[6] = int.Parse(RS6);
                        FailureRatePerYear[6] = ConvertMtbfToFr(Convert.ToDouble(FR6, ci));
                        RepairLossPerYear[6] = Convert.ToDouble(Convert.ToDouble(RL6, ci) / 100);

                        break;
                    case 7:
                        InstalledBasePerYear[7] = int.Parse(IB7);
                        RegionalStocksPerYear[7] = int.Parse(RS7);
                        FailureRatePerYear[7] = ConvertMtbfToFr(Convert.ToDouble(FR7, ci));
                        RepairLossPerYear[7] = Convert.ToDouble(Convert.ToDouble(RL7, ci) / 100);

                        break;
                    case 8:
                        InstalledBasePerYear[8] = int.Parse(IB8);
                        RegionalStocksPerYear[8] = int.Parse(RS8);
                        FailureRatePerYear[8] = ConvertMtbfToFr(Convert.ToDouble(FR8, ci));
                        RepairLossPerYear[8] = Convert.ToDouble(Convert.ToDouble(RL8, ci) / 100);

                        break;
                    case 9:
                        InstalledBasePerYear[9] = int.Parse(IB9);
                        RegionalStocksPerYear[9] = int.Parse(RS9);
                        FailureRatePerYear[9] = ConvertMtbfToFr(Convert.ToDouble(FR8, ci));
                        RepairLossPerYear[9] = Convert.ToDouble(Convert.ToDouble(RL9, ci) / 100);

                        break;
                }
                yearCnt++;
            }

            UpdateForecolorAndClearRemains(yearCnt,
                         ref IB0, ref IB1, ref IB2, ref IB3, ref IB4, ref IB5, ref IB6, ref IB7, ref IB8, ref IB9, ref IB10,
                         ref FR0, ref FR1, ref FR2, ref FR3, ref FR4, ref FR5, ref FR6, ref FR7, ref FR8, ref FR9,
                         ref RS0, ref RS1, ref RS2, ref RS3, ref RS4, ref RS5, ref RS6, ref RS7, ref RS8, ref RS9,
                         ref RL0, ref RL1, ref RL2, ref RL3, ref RL4, ref RL5, ref RL6, ref RL7, ref RL8, ref RL9);
        }

        private void UpdateForecolorAndClearRemains(int startYear,
                         ref string IB0, ref string IB1, ref string IB2, ref string IB3, ref string IB4, ref string IB5, ref string IB6, ref string IB7, ref string IB8, ref string IB9, ref string IB10,
                         ref string FR0, ref string FR1, ref string FR2, ref string FR3, ref string FR4, ref string FR5, ref string FR6, ref string FR7, ref string FR8, ref string FR9,
                         ref string RS0, ref string RS1, ref string RS2, ref string RS3, ref string RS4, ref string RS5, ref string RS6, ref string RS7, ref string RS8, ref string RS9,
                         ref string RL0, ref string RL1, ref string RL2, ref string RL3, ref string RL4, ref string RL5, ref string RL6, ref string RL7, ref string RL8, ref string RL9)
        {
            var yearCnt = startYear;
            while (yearCnt <= LtbCommon.MaxYear)
            {
                switch (yearCnt)
                {
                    case 0:
                        IB0 = string.Empty;
                        RS0 = string.Empty;
                        FR0 = string.Empty;
                        RL0 = string.Empty;
                        IB1 = string.Empty;
                        RS1 = string.Empty;
                        FR1 = string.Empty;
                        RL1 = string.Empty;
                        break;
                    case 1:
                        IB2 = string.Empty;
                        RS2 = string.Empty;
                        FR2 = string.Empty;
                        RL2 = string.Empty;
                        break;
                    case 2:
                        IB3 = string.Empty;
                        RS3 = string.Empty;
                        FR3 = string.Empty;
                        RL3 = string.Empty;
                        break;
                    case 3:
                        IB4 = string.Empty;
                        RS4 = string.Empty;
                        FR4 = string.Empty;
                        RL4 = string.Empty;
                        break;
                    case 4:
                        IB5 = string.Empty;
                        RS5 = string.Empty;
                        FR5 = string.Empty;
                        RL5 = string.Empty;
                        break;
                    case 5:
                        IB6 = string.Empty;
                        RS6 = string.Empty;
                        FR6 = string.Empty;
                        RL6 = string.Empty;
                        break;
                    case 6:
                        IB7 = string.Empty;
                        RS7 = string.Empty;
                        FR7 = string.Empty;
                        RL7 = string.Empty;
                        break;
                    case 7:
                        IB8 = string.Empty;
                        RS8 = string.Empty;
                        FR8 = string.Empty;
                        RL8 = string.Empty;
                        break;
                    case 8:
                        IB9 = string.Empty;
                        RS9 = string.Empty;
                        FR9 = string.Empty;
                        RL9 = string.Empty;
                        break;
                    case 9:
                        break;
                    case 10:
                        break;
                }
                yearCnt += 1;
            }
        }

        private bool CheckLimits(int finalYear, string LTBDate, string EOSDate, ref string infoText,
                         ref string IB0, ref string IB1, ref string IB2, ref string IB3, ref string IB4, ref string IB5, ref string IB6, ref string IB7, ref string IB8, ref string IB9, ref string IB10,
                         ref string FR0, ref string FR1, ref string FR2, ref string FR3, ref string FR4, ref string FR5, ref string FR6, ref string FR7, ref string FR8, ref string FR9,
                         ref string RS0, ref string RS1, ref string RS2, ref string RS3, ref string RS4, ref string RS5, ref string RS6, ref string RS7, ref string RS8, ref string RS9,
                         ref string RL0, ref string RL1, ref string RL2, ref string RL3, ref string RL4, ref string RL5, ref string RL6, ref string RL7, ref string RL8, ref string RL9)
        {
            var functionReturnValue = false;
            var tmpDate = Convert.ToDateTime(LTBDate);
            var yearCnt = 0;
            functionReturnValue = true;
            var ci = new CultureInfo("sv-SE");
            while (yearCnt <= finalYear)
            {
                var tmp = "";
                switch (yearCnt)
                {
                    case 0:
                        tmp = FR0;
                        if (tmp != "") tmp = Strings.Replace(tmp, ".", ",");
                        FR0 = tmp;
                        if (RS0 == string.Empty)
                            RS0 = "0";
                        if (!Information.IsNumeric(IB0) |
                            !Information.IsNumeric(RS0) |
                            !Information.IsNumeric(FR0) |
                            !Information.IsNumeric(RL0))
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = string.Format("Fel parameter för " + "{0}", tmpDate.Year);
                            return functionReturnValue;
                        }
                        if (Convert.ToInt32(IB0) > 999999 |
                            Convert.ToInt32(IB0) < 0 |
                            Convert.ToInt32(RS0) < 0 |
                            Convert.ToInt32(RS0) > 9999 |
                            ConvertMtbfToFr(Convert.ToDouble(FR0, ci)) < 1E-07 |
                            ConvertMtbfToFr(Convert.ToDouble(FR0, ci)) > 100 |
                            Convert.ToInt32(RL0) < 0 |
                            Convert.ToInt32(RL0) > 100)
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + tmpDate.Year;
                            return functionReturnValue;
                        }
                        break;
                    case 1:
                        tmp = FR1;
                        if (tmp != "") tmp = Strings.Replace(tmp, ".", ",");
                        FR1 = tmp;
                        if (RS1 == string.Empty)
                            RS1 = "0";
                        if (!Information.IsNumeric(IB1) |
                            !Information.IsNumeric(RS1) |
                            !Information.IsNumeric(FR1) |
                            !Information.IsNumeric(RL1))
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 1);
                            return functionReturnValue;
                        }
                        if (Convert.ToInt32(IB1) > 999999 |
                            Convert.ToInt32(IB1) < 0 |
                            Convert.ToInt32(RS1) < 0 |
                            Convert.ToInt32(RS1) > 9999 |
                            ConvertMtbfToFr(Convert.ToDouble(FR1, ci)) < 1E-06 |
                            ConvertMtbfToFr(Convert.ToDouble(FR1, ci)) > 100 |
                            Convert.ToInt32(RL1) < 0 |
                            Convert.ToInt32(RL1) > 100)
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 1);
                            return functionReturnValue;
                        }
                        break;
                    case 2:
                        tmp = FR2;
                        if (tmp != "") tmp = Strings.Replace(tmp, ".", ",");
                        FR2 = tmp;
                        if (RS2 == string.Empty)
                            RS2 = "0";
                        if (!Information.IsNumeric(IB2) |
                            !Information.IsNumeric(RS2) |
                            !Information.IsNumeric(FR2) |
                            !Information.IsNumeric(RL2))
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 2);
                            return functionReturnValue;
                        }
                        if (Convert.ToInt32(IB2) > 999999 |
                            Convert.ToInt32(IB2) < 0 |
                            Convert.ToInt32(RS2) < 0 |
                            Convert.ToInt32(RS2) > 9999 |
                            ConvertMtbfToFr(Convert.ToDouble(FR2, ci)) < 1E-06 |
                            ConvertMtbfToFr(Convert.ToDouble(FR2, ci)) > 100 |
                            Convert.ToInt32(RL2) < 0 |
                            Convert.ToInt32(RL2) > 100)
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 2);
                            return functionReturnValue;
                        }
                        break;
                    case 3:
                        tmp = FR3;
                        if (tmp != "") tmp = Strings.Replace(tmp, ".", ",");
                        FR3 = tmp;
                        if (RS3 == string.Empty)
                            RS3 = "0";
                        if (!Information.IsNumeric(IB3) |
                            !Information.IsNumeric(RS3) |
                            !Information.IsNumeric(FR3) |
                            !Information.IsNumeric(RL3))
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 3);
                            return functionReturnValue;
                        }
                        if (Convert.ToInt32(IB3) > 999999 |
                            Convert.ToInt32(IB3) < 0 |
                            Convert.ToInt32(RS3) < 0 |
                            Convert.ToInt32(RS3) > 9999 |
                            ConvertMtbfToFr(Convert.ToDouble(FR3, ci)) < 1E-06 |
                            ConvertMtbfToFr(Convert.ToDouble(FR3, ci)) > 100 |
                            Convert.ToInt32(RL3) < 0 |
                            Convert.ToInt32(RL3) > 100)
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 3);
                            return functionReturnValue;
                        }
                        break;
                    case 4:
                        tmp = FR4;
                        if (tmp != "") tmp = Strings.Replace(tmp, ".", ",");
                        FR4 = tmp;
                        if (RS4 == string.Empty)
                            RS4 = "0";
                        if (!Information.IsNumeric(IB4) |
                            !Information.IsNumeric(RS4) |
                            !Information.IsNumeric(FR4) |
                            !Information.IsNumeric(RL4))
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 4);
                            return functionReturnValue;
                        }
                        if (Convert.ToInt32(IB4) > 999999 |
                            Convert.ToInt32(IB4) < 0 |
                            Convert.ToInt32(RS4) < 0 |
                            Convert.ToInt32(RS4) > 9999 |
                            ConvertMtbfToFr(Convert.ToDouble(FR4, ci)) < 1E-06 |
                            ConvertMtbfToFr(Convert.ToDouble(FR4, ci)) > 100 |
                            Convert.ToInt32(RL4) < 0 |
                            Convert.ToInt32(RL4) > 100)
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 4);
                            return functionReturnValue;
                        }
                        break;
                    case 5:
                        tmp = FR5;
                        if (tmp != "") tmp = Strings.Replace(tmp, ".", ",");
                        FR5 = tmp;
                        if (RS5 == string.Empty)
                            RS5 = "0";
                        if (!Information.IsNumeric(IB5) |
                            !Information.IsNumeric(RS5) |
                            !Information.IsNumeric(FR5) |
                            !Information.IsNumeric(RL5))
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 5);
                            return functionReturnValue;
                        }
                        if (Convert.ToInt32(IB5) > 999999 |
                            Convert.ToInt32(IB5) < 0 |
                            Convert.ToInt32(RS5) < 0 |
                            Convert.ToInt32(RS5) > 9999 |
                            ConvertMtbfToFr(Convert.ToDouble(FR5, ci)) < 1E-06 |
                            ConvertMtbfToFr(Convert.ToDouble(FR5, ci)) > 100 |
                            Convert.ToInt32(RL5) < 0 |
                            Convert.ToInt32(RL5) > 100)
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 5);
                            return functionReturnValue;
                        }
                        break;
                    case 6:
                        tmp = FR6;
                        if (tmp != "") tmp = Strings.Replace(tmp, ".", ",");
                        FR6 = tmp;
                        if (RS6 == string.Empty)
                            RS6 = "0";
                        if (!Information.IsNumeric(IB6) |
                            !Information.IsNumeric(RS6) |
                            !Information.IsNumeric(FR6) |
                            !Information.IsNumeric(RL6))
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 6);
                            return functionReturnValue;
                        }
                        if (Convert.ToInt32(IB6) > 999999 |
                            Convert.ToInt32(IB6) < 0 |
                            Convert.ToInt32(RS6) < 0 |
                            Convert.ToInt32(RS6) > 9999 |
                            ConvertMtbfToFr(Convert.ToDouble(FR6, ci)) < 1E-06 |
                            ConvertMtbfToFr(Convert.ToDouble(FR6, ci)) > 100 |
                            Convert.ToInt32(RL6) < 0 |
                            Convert.ToInt32(RL6) > 100)
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 6);
                            return functionReturnValue;
                        }
                        break;
                    case 7:
                        tmp = FR7;
                        if (tmp != "") tmp = Strings.Replace(tmp, ".", ",");
                        FR7 = tmp;
                        if (RS7 == string.Empty)
                            RS7 = "0";
                        if (!Information.IsNumeric(IB7) |
                            !Information.IsNumeric(RS7) |
                            !Information.IsNumeric(FR7) |
                            !Information.IsNumeric(RL7))
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 7);
                            return functionReturnValue;
                        }
                        if (Convert.ToInt32(IB7) > 999999 |
                            Convert.ToInt32(IB7) < 0 |
                            Convert.ToInt32(RS7) < 0 |
                            Convert.ToInt32(RS7) > 9999 |
                            ConvertMtbfToFr(Convert.ToDouble(FR7, ci)) < 1E-06 |
                            ConvertMtbfToFr(Convert.ToDouble(FR7, ci)) > 100 |
                            Convert.ToInt32(RL7) < 0 |
                            Convert.ToInt32(RL7) > 100)
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 7);
                            return functionReturnValue;
                        }
                        break;
                    case 8:
                        tmp = FR8;
                        if (tmp != "") tmp = Strings.Replace(tmp, ".", ",");
                        FR8 = tmp;
                        if (RS8 == string.Empty)
                            RS8 = "0";
                        if (!Information.IsNumeric(IB8) |
                            !Information.IsNumeric(RS8) |
                            !Information.IsNumeric(FR8) |
                            !Information.IsNumeric(RL8))
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 8);
                            return functionReturnValue;
                        }
                        if (Convert.ToInt32(IB8) > 999999 |
                            Convert.ToInt32(IB8) < 0 |
                            Convert.ToInt32(RS8) < 0 |
                            Convert.ToInt32(RS8) > 9999 |
                            ConvertMtbfToFr(Convert.ToDouble(FR8, ci)) < 1E-06 |
                            ConvertMtbfToFr(Convert.ToDouble(FR8, ci)) > 100 |
                            Convert.ToInt32(RL8) < 0 |
                            Convert.ToInt32(RL8) > 100)
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 8);
                            return functionReturnValue;
                        }
                        break;
                    case 9:
                        tmp = FR9;
                        if (tmp != "") tmp = Strings.Replace(tmp, ".", ",");
                        FR9 = tmp;
                        if (RS9 == string.Empty)
                            RS9 = "0";
                        if (!Information.IsNumeric(IB9) |
                            !Information.IsNumeric(RS9) |
                            !Information.IsNumeric(FR9) |
                            !Information.IsNumeric(RL9))
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 9);
                            return functionReturnValue;
                        }
                        if (Convert.ToInt32(IB9) > 999999 |
                            Convert.ToInt32(IB9) < 0 |
                            Convert.ToInt32(RS9) < 0 |
                            Convert.ToInt32(RS9) > 9999 |
                            ConvertMtbfToFr(Convert.ToDouble(FR9, ci)) < 1E-06 |
                            ConvertMtbfToFr(Convert.ToDouble(FR9, ci)) > 100 |
                            Convert.ToInt32(RL9) < 0 |
                            Convert.ToInt32(RL9) > 100)
                        {
                            functionReturnValue = false;
                            yearCnt = finalYear;
                            infoText = "Fel parameter för " + (Convert.ToInt32(tmpDate.Year) + 9);
                            return functionReturnValue;
                        }
                        break;
                    case 10:
                        break;
                }
                yearCnt += 1;
            }
            return functionReturnValue;
        }

        [HttpPost]
        public ActionResult DateChanged(string confidence, string repairLeadTime, bool repairPossible, bool mtbfSelected, string LTBDate, string EOSDate,
                          string IB0, string IB1, string IB2, string IB3, string IB4, string IB5, string IB6, string IB7, string IB8, string IB9, string IB10,
                          string FR0, string FR1, string FR2, string FR3, string FR4, string FR5, string FR6, string FR7, string FR8, string FR9,
                          string RS0, string RS1, string RS2, string RS3, string RS4, string RS5, string RS6, string RS7, string RS8, string RS9,
                          string RL0, string RL1, string RL2, string RL3, string RL4, string RL5, string RL6, string RL7, string RL8, string RL9)
        {
            string infoText = string.Empty;
            if (LTBDate == null || EOSDate == null)
            {
                LTBDate = DateTime.Now.ToString("yyyy-MM-dd");
                EOSDate = DateTime.Now.AddDays(3652).ToString("yyyy-MM-dd");
                infoText = "Välj datum för Life Time Buy och End Of Service, ange parametervärden och tryck 'Beräkna'.";
            }

            var startDate = Convert.ToDateTime(LTBDate);
            var endDate = Convert.ToDateTime(EOSDate);
            bool inputError = false;

            if (DateTimeUtil.DateDiff(DateTimeUtil.DateInterval.Day, startDate, endDate) > LtbCommon.MaxServiceDays)
            {
                EOSDate = startDate.AddDays(LtbCommon.MaxServiceDays).ToString("YYYY-MM-DD");
                infoText = "Fel: Serviceperioden får ej vara längre än 10 år. Vänligen ändra EoS eller LTB";
                inputError = true;
            }
            else if (DateTimeUtil.DateDiff(DateTimeUtil.DateInterval.Day, startDate, endDate) < LtbCommon.MinLeadDays)
            {
                EOSDate = startDate.AddDays(LtbCommon.MinLeadDays).ToString("YYYY-MM-DD");
                infoText = "Fel: Serviceperioden får ej vara längre än 10 år. Vänligen ändra EoS eller LTB";
                inputError = true;
            }

            LtbResult result = new LtbResult();
            if (!inputError)
            {
                _ltbChart = null;
                int serviceDays = GetServiceDays(LTBDate, EOSDate);
                int serviceYears = GetServiceYears(LTBDate, EOSDate);
                string IB1ForeColor = "";
                bool IB1Disabled = false;
                string IB2ForeColor = "";
                bool IB2Disabled = false;
                string IB3ForeColor = "";
                bool IB3Disabled = false;
                string IB4ForeColor = "";
                bool IB4Disabled = false;
                string IB5ForeColor = "";
                bool IB5Disabled = false;
                string IB6ForeColor = "";
                bool IB6Disabled = false;
                string IB7ForeColor = "";
                bool IB7Disabled = false;
                string IB8ForeColor = "";
                bool IB8Disabled = false;
                string IB9ForeColor = "";
                bool IB9Disabled = false;
                bool FR1Disabled = false;
                bool FR2Disabled = false;
                bool FR3Disabled = false;
                bool FR4Disabled = false;
                bool FR5Disabled = false;
                bool FR6Disabled = false;
                bool FR7Disabled = false;
                bool FR8Disabled = false;
                bool FR9Disabled = false;
                bool RS1Disabled = false;
                bool RS2Disabled = false;
                bool RS3Disabled = false;
                bool RS4Disabled = false;
                bool RS5Disabled = false;
                bool RS6Disabled = false;
                bool RS7Disabled = false;
                bool RS8Disabled = false;
                bool RS9Disabled = false;
                bool RL0Disabled = false;
                bool RL1Disabled = false;
                bool RL2Disabled = false;
                bool RL3Disabled = false;
                bool RL4Disabled = false;
                bool RL5Disabled = false;
                bool RL6Disabled = false;
                bool RL7Disabled = false;
                bool RL8Disabled = false;
                bool RL9Disabled = false;
                UpdateYearTabIndex(serviceYears, _mtbfSelected, repairPossible,
                            ref IB0, ref IB1, ref IB2, ref IB3, ref IB4, ref IB5, ref IB6, ref IB7, ref IB8, ref IB9, ref IB10,
                            ref FR0, ref FR1, ref FR2, ref FR3, ref FR4, ref FR5, ref FR6, ref FR7, ref FR8, ref FR9,
                            ref RS0, ref RS1, ref RS2, ref RS3, ref RS4, ref RS5, ref RS6, ref RS7, ref RS8, ref RS9,
                            ref RL0, ref RL1, ref RL2, ref RL3, ref RL4, ref RL5, ref RL6, ref RL7, ref RL8, ref RL9,
                            ref IB1ForeColor, ref IB2ForeColor, ref IB3ForeColor, ref IB4ForeColor, ref IB5ForeColor, ref IB6ForeColor, ref IB7ForeColor, ref IB8ForeColor, ref IB9ForeColor,
                            ref IB1Disabled, ref IB2Disabled, ref IB3Disabled, ref IB4Disabled, ref IB5Disabled, ref IB6Disabled, ref IB7Disabled, ref IB8Disabled, ref IB9Disabled,
                            ref FR1Disabled, ref FR2Disabled, ref FR3Disabled, ref FR4Disabled, ref FR5Disabled, ref FR6Disabled, ref FR7Disabled, ref FR8Disabled, ref FR9Disabled,
                            ref RS1Disabled, ref RS2Disabled, ref RS3Disabled, ref RS4Disabled, ref RS5Disabled, ref RS6Disabled, ref RS7Disabled, ref RS8Disabled, ref RS9Disabled,
                            ref RL0Disabled, ref RL1Disabled, ref RL2Disabled, ref RL3Disabled, ref RL4Disabled, ref RL5Disabled, ref RL6Disabled, ref RL7Disabled, ref RL8Disabled, ref RL9Disabled);

                UpdateRepair(serviceYears, repairPossible,
                            ref RL0, ref RL1, ref RL2, ref RL3, ref RL4, ref RL5, ref RL6, ref RL7, ref RL8, ref RL9,
                            ref RL0Disabled, ref RL1Disabled, ref RL2Disabled, ref RL3Disabled, ref RL4Disabled, ref RL5Disabled, ref RL6Disabled, ref RL7Disabled, ref RL8Disabled, ref RL9Disabled);

                result.stock = "";
                result.safety = "";
                result.total = "";
                result.failed = "";
                result.repaired = "";
                result.lost = "";
                result.infoText = infoText;
                result.servicedays = serviceDays.ToString();
                result.LTBDate = LTBDate;
                result.EOSDate = EOSDate;
                result.mtbfSelected = mtbfSelected;
                result.repairPossible = repairPossible;
                result.IB0 = IB0;
                result.IB1 = IB1;
                result.IB2 = IB2;
                result.IB3 = IB3;
                result.IB4 = IB4;
                result.IB5 = IB5;
                result.IB6 = IB6;
                result.IB7 = IB7;
                result.IB8 = IB8;
                result.IB9 = IB9;
                result.IB10 = IB10;
                result.FR0 = FR0;
                result.FR1 = FR1;
                result.FR2 = FR2;
                result.FR3 = FR3;
                result.FR4 = FR4;
                result.FR5 = FR5;
                result.FR6 = FR6;
                result.FR7 = FR7;
                result.FR8 = FR8;
                result.FR9 = FR9;
                result.RS0 = RS0;
                result.RS1 = RS1;
                result.RS2 = RS2;
                result.RS3 = RS3;
                result.RS4 = RS4;
                result.RS5 = RS5;
                result.RS6 = RS6;
                result.RS7 = RS7;
                result.RS8 = RS8;
                result.RS9 = RS9;
                result.RL0 = RL0;
                result.RL1 = RL1;
                result.RL2 = RL2;
                result.RL3 = RL3;
                result.RL4 = RL4;
                result.RL5 = RL5;
                result.RL6 = RL6;
                result.RL7 = RL7;
                result.RL8 = RL8;
                result.RL9 = RL9;
                result.IB1ForeColor = IB1ForeColor;
                result.IB1Disabled = IB1Disabled;
                result.IB2ForeColor = IB2ForeColor;
                result.IB2Disabled = IB2Disabled;
                result.IB3ForeColor = IB3ForeColor;
                result.IB3Disabled = IB3Disabled;
                result.IB4ForeColor = IB4ForeColor;
                result.IB4Disabled = IB4Disabled;
                result.IB5ForeColor = IB5ForeColor;
                result.IB5Disabled = IB5Disabled;
                result.IB6ForeColor = IB6ForeColor;
                result.IB6Disabled = IB6Disabled;
                result.IB7ForeColor = IB7ForeColor;
                result.IB7Disabled = IB7Disabled;
                result.IB8ForeColor = IB8ForeColor;
                result.IB8Disabled = IB8Disabled;
                result.IB9ForeColor = IB9ForeColor;
                result.IB9Disabled = IB9Disabled;
                result.FR1Disabled = FR1Disabled;
                result.FR2Disabled = FR2Disabled;
                result.FR3Disabled = FR3Disabled;
                result.FR4Disabled = FR4Disabled;
                result.FR5Disabled = FR5Disabled;
                result.FR6Disabled = FR6Disabled;
                result.FR7Disabled = FR7Disabled;
                result.FR8Disabled = FR8Disabled;
                result.FR9Disabled = FR9Disabled;
                result.RS1Disabled = RS1Disabled;
                result.RS2Disabled = RS2Disabled;
                result.RS3Disabled = RS3Disabled;
                result.RS4Disabled = RS4Disabled;
                result.RS5Disabled = RS5Disabled;
                result.RS6Disabled = RS6Disabled;
                result.RS7Disabled = RS7Disabled;
                result.RS8Disabled = RS8Disabled;
                result.RS9Disabled = RS9Disabled;
                result.RL0Disabled = RL0Disabled;
                result.RL1Disabled = RL1Disabled;
                result.RL2Disabled = RL2Disabled;
                result.RL3Disabled = RL3Disabled;
                result.RL4Disabled = RL4Disabled;
                result.RL5Disabled = RL5Disabled;
                result.RL6Disabled = RL6Disabled;
                result.RL7Disabled = RL7Disabled;
                result.RL8Disabled = RL8Disabled;
                result.RL9Disabled = RL9Disabled;
            }
            else
            {
                result.infoText = infoText;
            }

            return Json(result);

        }

        private void UpdateYearTabIndex(int serviceYears, bool _mtbfSelected, bool repairPossible,
            ref string IB0, ref string IB1, ref string IB2, ref string IB3, ref string IB4, ref string IB5, ref string IB6, ref string IB7, ref string IB8, ref string IB9, ref string IB10,
            ref string FR0, ref string FR1, ref string FR2, ref string FR3, ref string FR4, ref string FR5, ref string FR6, ref string FR7, ref string FR8, ref string FR9,
            ref string RS0, ref string RS1, ref string RS2, ref string RS3, ref string RS4, ref string RS5, ref string RS6, ref string RS7, ref string RS8, ref string RS9,
            ref string RL0, ref string RL1, ref string RL2, ref string RL3, ref string RL4, ref string RL5, ref string RL6, ref string RL7, ref string RL8, ref string RL9,
            ref string IB1ForeColor, ref string IB2ForeColor, ref string IB3ForeColor, ref string IB4ForeColor, ref string IB5ForeColor, ref string IB6ForeColor, ref string IB7ForeColor, ref string IB8ForeColor, ref string IB9ForeColor,
            ref bool IB1Disabled, ref bool IB2Disabled, ref bool IB3Disabled, ref bool IB4Disabled, ref bool IB5Disabled, ref bool IB6Disabled, ref bool IB7Disabled, ref bool IB8Disabled, ref bool IB9Disabled,
            ref bool FR1Disabled, ref bool FR2Disabled, ref bool FR3Disabled, ref bool FR4Disabled, ref bool FR5Disabled, ref bool FR6Disabled, ref bool FR7Disabled, ref bool FR8Disabled, ref bool FR9Disabled,
            ref bool RS1Disabled, ref bool RS2Disabled, ref bool RS3Disabled, ref bool RS4Disabled, ref bool RS5Disabled, ref bool RS6Disabled, ref bool RS7Disabled, ref bool RS8Disabled, ref bool RS9Disabled,
            ref bool RL0Disabled, ref bool RL1Disabled, ref bool RL2Disabled, ref bool RL3Disabled, ref bool RL4Disabled, ref bool RL5Disabled, ref bool RL6Disabled, ref bool RL7Disabled, ref bool RL8Disabled, ref bool RL9Disabled)
        {
            var yearCnt = 0;
            while (yearCnt <= serviceYears)
            {
                switch (yearCnt)
                {
                    case 0:
                        if (IB1 == "EoS") IB1 = string.Empty;
                        IB1ForeColor = "black40";
                        if (string.IsNullOrEmpty(RS1))
                        {
                            RS1 = RS0;
                            FR1 = FR0;
                            IB1 = IB0;
                            RL1 = RL0;
                        }
                        IB1Disabled = false;
                        RS1Disabled = false;
                        FR1Disabled = false;
                        RL1Disabled = false;
                        break;
                    case 1:
                        if (IB2 == "EoS") IB2 = string.Empty;
                        IB2ForeColor = "black40";
	                    if (string.IsNullOrEmpty(RS2))
						{
                            RS2 = RS1;
                            FR2 = FR1;
                            IB2 = IB1;
                            RL2 = RL1;
                        }
                        IB2Disabled = false;
                        RS2Disabled = false;
                        FR2Disabled = false;
                        RL2Disabled = false;
                        break;
                    case 2:
                        if (IB3 == "EoS") IB3 = string.Empty;
                        IB3ForeColor = "black40";
	                    if (string.IsNullOrEmpty(RS3))
						{
                            RS3 = RS2;
                            FR3 = FR2;
                            IB3 = IB2;
                            RL3 = RL2;
                        }
                        IB3Disabled = false;
                        RS3Disabled = false;
                        FR3Disabled = false;
                        RL3Disabled = false;
                        break;
                    case 3:
                        if (IB4 == "EoS") IB4 = string.Empty;
                        IB4ForeColor = "black40";
	                    if (string.IsNullOrEmpty(RS4))
						{
                            RS4 = RS3;
                            FR4 = FR3;
                            IB4 = IB3;
                            RL4 = RL3;
                        }
                        IB4Disabled = false;
                        RS4Disabled = false;
                        FR4Disabled = false;
                        RL4Disabled = false;
                        break;
                    case 4:
                        if (IB5 == "EoS") IB5 = string.Empty;
                        IB5ForeColor = "black40";
	                    if (string.IsNullOrEmpty(RS5))
						{
                            RS5 = RS4;
                            FR5 = FR4;
                            IB5 = IB4;
                            RL5 = RL4;
                        }
                        IB5Disabled = false;
                        RS5Disabled = false;
                        FR5Disabled = false;
                        RL5Disabled = false;
                        break;
                    case 5:
                        if (IB6 == "EoS") IB6 = string.Empty;
                        IB6ForeColor = "black40";
	                    if (string.IsNullOrEmpty(RS6))
						{
                            RS6 = RS5;
                            FR6 = FR5;
                            IB6 = IB5;
                            RL6 = RL5;
                        }
                        IB6Disabled = false;
                        RS6Disabled = false;
                        FR6Disabled = false;
                        RL6Disabled = false;
                        break;
                    case 6:
                        if (IB7 == "EoS") IB7 = string.Empty;
                        IB7ForeColor = "black40";
	                    if (string.IsNullOrEmpty(RS7))
						{
                            RS7 = RS6;
                            FR7 = FR6;
                            IB7 = IB6;
                            RL7 = RL6;
                        }
                        IB7Disabled = false;
                        RS7Disabled = false;
                        FR7Disabled = false;
                        RL7Disabled = false;
                        break;
                    case 7:
                        if (IB8 == "EoS") IB8 = string.Empty;
                        IB8ForeColor = "black40";
	                    if (string.IsNullOrEmpty(RS8))
						{
                            RS8 = RS7;
                            FR8 = FR7;
                            IB8 = IB7;
                            RL8 = RL7;
                        }
                        IB8Disabled = false;
                        RS8Disabled = false;
                        FR8Disabled = false;
                        RL8Disabled = false;
                        break;
                    case 8:
                        if (IB9 == "EoS") IB9 = string.Empty;
                        IB9ForeColor = "black40";
	                    if (string.IsNullOrEmpty(RS9))
						{
                            RS9 = RS8;
                            FR9 = FR8;
                            IB9 = IB8;
                            RL9 = RL8;
                        }
                        IB9Disabled = false;
                        RS9Disabled = false;
                        FR9Disabled = false;
                        RL9Disabled = false;
                        break;
                    case 9:
                        break;
                }
                yearCnt += 1;
            }

            switch (serviceYears)
            {
                case 0:
                    IB1 = "EoS";
                    IB1ForeColor = "red40";
                    IB2ForeColor = "red40";
                    IB3ForeColor = "red40";
                    IB4ForeColor = "red40";
                    IB5ForeColor = "red40";
                    IB6ForeColor = "red40";
                    IB7ForeColor = "red40";
                    IB8ForeColor = "red40";
                    IB9ForeColor = "red40";
                    break;
                case 1:
                    IB2 = "EoS";
                    IB2ForeColor = "red40";
                    IB3ForeColor = "red40";
                    IB4ForeColor = "red40";
                    IB5ForeColor = "red40";
                    IB6ForeColor = "red40";
                    IB7ForeColor = "red40";
                    IB8ForeColor = "red40";
                    IB9ForeColor = "red40";
                    break;
                case 2:
                    IB3 = "EoS";
                    IB3ForeColor = "red40";
                    IB4ForeColor = "red40";
                    IB5ForeColor = "red40";
                    IB6ForeColor = "red40";
                    IB7ForeColor = "red40";
                    IB8ForeColor = "red40";
                    IB9ForeColor = "red40";
                    break;
                case 3:
                    IB4 = "EoS";
                    IB4ForeColor = "red40";
                    IB5ForeColor = "red40";
                    IB6ForeColor = "red40";
                    IB7ForeColor = "red40";
                    IB8ForeColor = "red40";
                    IB9ForeColor = "red40";
                    break;
                case 4:
                    IB5 = "EoS";
                    IB5ForeColor = "red40";
                    IB6ForeColor = "red40";
                    IB7ForeColor = "red40";
                    IB8ForeColor = "red40";
                    IB9ForeColor = "red40";
                    break;
                case 5:
                    IB6 = "EoS";
                    IB6ForeColor = "red40";
                    IB7ForeColor = "red40";
                    IB8ForeColor = "red40";
                    IB9ForeColor = "red40";
                    break;
                case 6:
                    IB7 = "EoS";
                    IB7ForeColor = "red40";
                    IB8ForeColor = "red40";
                    IB9ForeColor = "red40";
                    break;
                case 7:
                    IB8 = "EoS";
                    IB8ForeColor = "red40";
                    IB9ForeColor = "red40";
                    break;
                case 8:
                    IB9 = "EoS";
                    IB9ForeColor = "red40";
                    break;
                case 9:
                    IB10 = "EoS";
                    break;
                case 10:
                    break;
            }

            while (yearCnt <= LtbCommon.MaxYear)
            {
                switch (yearCnt)
                {
                    case 1:
                        if (serviceYears != 0)
                        {
                            IB1 = string.Empty;
                            IB1Disabled = true;
                        }
                        RS1 = string.Empty;
                        FR1 = string.Empty;
                        RL1 = string.Empty;
                        RS1Disabled = true;
                        FR1Disabled = true;
                        RL1Disabled = true;

                        break;
                    case 2:
                        if (serviceYears != 1)
                        {
                            IB2 = string.Empty;
                            IB2Disabled = true;
                        }
                        RS2 = string.Empty;
                        FR2 = string.Empty;
                        RL2 = string.Empty;
                        RS2Disabled = true;
                        FR2Disabled = true;
                        RL2Disabled = true;
                        break;
                    case 3:
                        if (serviceYears != 2)
                        {
                            IB3 = string.Empty;
                            IB3Disabled = true;
                        }
                        RS3 = string.Empty;
                        FR3 = string.Empty;
                        RL3 = string.Empty;
                        RS3Disabled = true;
                        FR3Disabled = true;
                        RL3Disabled = true;
                        break;
                    case 4:
                        if (serviceYears != 3)
                        {
                            IB4 = string.Empty;
                            IB4Disabled = true;
                        }
                        RS4 = string.Empty;
                        FR4 = string.Empty;
                        RL4 = string.Empty;
                        RS4Disabled = true;
                        FR4Disabled = true;
                        RL4Disabled = true;
                        break;
                    case 5:
                        if (serviceYears != 4)
                        {
                            IB5 = string.Empty;
                            IB5Disabled = true;
                        }
                        RS5 = string.Empty;
                        FR5 = string.Empty;
                        RL5 = string.Empty;
                        RS5Disabled = true;
                        FR5Disabled = true;
                        RL5Disabled = true;
                        break;
                    case 6:
                        if (serviceYears != 5)
                        {
                            IB6 = string.Empty;
                            IB6Disabled = true;
                        }
                        RS6 = string.Empty;
                        FR6 = string.Empty;
                        RL6 = string.Empty;
                        RS6Disabled = true;
                        FR6Disabled = true;
                        RL6Disabled = true;
                        break;
                    case 7:
                        if (serviceYears != 6)
                        {
                            IB7 = string.Empty;
                            IB7Disabled = true;
                        }
                        RS7 = string.Empty;
                        FR7 = string.Empty;
                        RL7 = string.Empty;
                        RS7Disabled = true;
                        FR7Disabled = true;
                        RL7Disabled = true;
                        break;
                    case 8:
                        if (serviceYears != 7)
                        {
                            IB8 = string.Empty;
                            IB8Disabled = true;
                        }
                        RS8 = string.Empty;
                        FR8 = string.Empty;
                        RL8 = string.Empty;
                        RS8Disabled = true;
                        FR8Disabled = true;
                        RL8Disabled = true;
                        break;
                    case 9:
                        if (serviceYears != 8)
                        {
                            IB9 = string.Empty;
                            IB9Disabled = true;
                        }
                        RS9 = string.Empty;
                        FR9 = string.Empty;
                        RL9 = string.Empty;
                        RS9Disabled = true;
                        FR9Disabled = true;
                        RL9Disabled = true;
                        break;
                    case 10:
                        if (serviceYears != 9)
                        {
                            IB10 = string.Empty;
                        }
                        break;
                }
                yearCnt += 1;
            }
        }

        private void UpdateRepair(int serviceYears, bool repairPossible,
            ref string RL0, ref string RL1, ref string RL2, ref string RL3, ref string RL4, ref string RL5, ref string RL6, ref string RL7, ref string RL8, ref string RL9,
            ref bool RL0Disabled, ref bool RL1Disabled, ref bool RL2Disabled, ref bool RL3Disabled, ref bool RL4Disabled, ref bool RL5Disabled, ref bool RL6Disabled, ref bool RL7Disabled, ref bool RL8Disabled, ref bool RL9Disabled)
        {
            var cnt = 0;
            while (cnt <= serviceYears)
            {
                switch (cnt)
                {
                    case 0:
                        if (!repairPossible)
                        {
                            RL0Disabled = true;
                            RL0 = "100";
                        }
                        else
                        {
                            RL0Disabled = false;
                        }
                        break;
                    case 1:
                        if (!repairPossible)
                        {
                            RL1Disabled = true;
                            RL1 = "100";
                        }
                        else
                        {
                            RL1Disabled = false;
                        }
                        break;
                    case 2:
                        if (!repairPossible)
                        {
                            RL2Disabled = true;
                            RL2 = "100";
                        }
                        else
                        {
                            RL2Disabled = false;
                        }
                        break;
                    case 3:
                        if (!repairPossible)
                        {
                            RL3Disabled = true;
                            RL3 = "100";
                        }
                        else
                        {
                            RL3Disabled = false;
                        }
                        break;
                    case 4:
                        if (!repairPossible)
                        {
                            RL4Disabled = true;
                            RL4 = "100";
                        }
                        else
                        {
                            RL4Disabled = false;
                        }
                        break;
                    case 5:
                        if (!repairPossible)
                        {
                            RL5Disabled = true;
                            RL5 = "100";
                        }
                        else
                        {
                            RL5Disabled = false;
                        }
                        break;
                    case 6:
                        if (!repairPossible)
                        {
                            RL6Disabled = true;
                            RL6 = "100";
                        }
                        else
                        {
                            RL6Disabled = false;
                        }
                        break;
                    case 7:
                        if (!repairPossible)
                        {
                            RL7Disabled = true;
                            RL7 = "100";
                        }
                        else
                        {
                            RL7Disabled = false;
                        }
                        break;
                    case 8:
                        if (!repairPossible)
                        {
                            RL8Disabled = true;
                            RL8 = "100";
                        }
                        else
                        {
                            RL8Disabled = false;
                        }
                        break;
                    case 9:
                        if (!repairPossible)
                        {
                            RL9Disabled = true;
                            RL9 = "100";
                        }
                        else
                        {
                            RL9Disabled = false;
                        }

                        break;
                }
                cnt += 1;
            }
        }

        [HttpPost]
        public ActionResult Calculate(string confidence, string repairLeadTime, bool repairPossible, bool mtbfselected, string LTBDate, string EOSDate,
                         string IB0, string IB1, string IB2, string IB3, string IB4, string IB5, string IB6, string IB7, string IB8, string IB9, string IB10,
                         string FR0, string FR1, string FR2, string FR3, string FR4, string FR5, string FR6, string FR7, string FR8, string FR9,
                         string RS0, string RS1, string RS2, string RS3, string RS4, string RS5, string RS6, string RS7, string RS8, string RS9,
                         string RL0, string RL1, string RL2, string RL3, string RL4, string RL5, string RL6, string RL7, string RL8, string RL9)
        {
            bool inputError = false;
            string infoText = string.Empty;
            _mtbfSelected = mtbfselected;
            _repairSelected = repairPossible;
            if (repairLeadTime == string.Empty)
            {
                infoText = "Reparationsledtid kan inte vara tom!";
                inputError = true;
            }
            _leadDays = (_repairSelected) ? Convert.ToInt32(repairLeadTime) : 2;
            if (_leadDays < LtbCommon.MinLeadDays | _leadDays > LtbCommon.MaxLeadDays)
            {
                infoText = "Fel: 2 <= Reparationsledtid <=365";
                inputError = true;

            }
            int _serviceDays = GetServiceDays(LTBDate, EOSDate);
            int _serviceYears = GetServiceYears(LTBDate, EOSDate);
            if (_leadDays > _serviceDays)
            {
                infoText = "Fel: Reparationsledtid kan inte vara längre än serviceperioden. Vänligen ändra EoS eller Reparationsledtid";
                inputError = true;

            }

            if (_serviceDays > LtbCommon.MaxServiceDays)
            {
                infoText = "Fel: Serviceperioden får ej vara längre än 10 år. Vänligen ändra EoS eller LTB";
                inputError = true;

            }

            if (!CheckLimits(_serviceYears, LTBDate, EOSDate, ref infoText,
                              ref IB0, ref IB1, ref IB2, ref IB3, ref IB4, ref IB5, ref IB6, ref IB7, ref IB8, ref IB9, ref IB10,
                              ref FR0, ref FR1, ref FR2, ref FR3, ref FR4, ref FR5, ref FR6, ref FR7, ref FR8, ref FR9,
                              ref RS0, ref RS1, ref RS2, ref RS3, ref RS4, ref RS5, ref RS6, ref RS7, ref RS8, ref RS9,
                              ref RL0, ref RL1, ref RL2, ref RL3, ref RL4, ref RL5, ref RL6, ref RL7, ref RL8, ref RL9))
            {
                inputError = true;
            }


            ConvertInput(_serviceYears, ref _confidenceLevelFromNormsInv, ref _confidenceLevelConverted, confidence, repairLeadTime, repairPossible, mtbfselected, LTBDate, EOSDate,
                         IB0, IB1, IB2, IB3, IB4, IB5, IB6, IB7, IB8, IB9, IB10,
                         FR0, FR1, FR2, FR3, FR4, FR5, FR6, FR7, FR8, FR9,
                         RS0, RS1, RS2, RS3, RS4, RS5, RS6, RS7, RS8, RS9,
                         RL0, RL1, RL2, RL3, RL4, RL5, RL6, RL7, RL8, RL9);
            LtbResult result = new LtbResult();
            if (!inputError)
            {
                var path = Server.MapPath(@"~/Upload");
                var ltb = new LtbCommon(800, 14);
                ltb.LtbWorker(_serviceDays,
                    _serviceYears,
                    _leadDays,
                    out _stock,
                    out _safety,
                    out _failed,
                    out _repaired,
                    out _lost,
                    out _total,
                    _confidenceLevelFromNormsInv,
                    _confidenceLevelConverted,
                    InstalledBasePerYear,
                    FailureRatePerYear,
                    RepairLossPerYear,
                    RegionalStocksPerYear,
                    out _ltbChart,
                    path);

                result.stock = _stock;
                result.safety = _safety;
                result.total = _total;
                result.failed = _failed;
                result.repaired = _repaired;
                result.lost = _lost;
                result.servicedays = _serviceDays.ToString();
                result.infoText = "Beräkning klar!";
            }
            else
            {
                result.infoText = infoText;
            }

            return Json(result);
        }

        [HttpGet]
        public FileContentResult Chart()
        {
            return (_ltbChart == null) ? ClearedChart : File(_ltbChart.GetBuffer(), "image/png");
        }

        double ConvertMtbfToFr(double d)
        {
            if (_mtbfSelected)
            {
                return 1 / d;
            }
            return d;
        }    
        
        private static bool IsLeapYear(long year)
        {
            return (year > 0) && (year % 4) == 0 && !((year % 100) == 0 && (year % 400) != 0);
        }

        private static long CountLeaps(long year)
        {
            return (year - 1) / 4 - (year - 1) / 100 + (year - 1) / 400;
        }
    }
}