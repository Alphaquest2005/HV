﻿using RMSDataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Transactions.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Aviad.WPF.Controls;
using Common.Core.Logging;
using Demo.WindowsPresentation.CustomMarkers;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using log4netWrapper;
using Microsoft.Win32;

using SimpleMvvmToolkit;
using TrackableEntities;


namespace SalesRegion
{

    /// <summary>
    /// Interaction logic for SalesTaskPad.xaml
    /// </summary>
    public partial class SalesTaskPad : UserControl
    {
        private static readonly SalesTaskPad _instance;

        static SalesTaskPad()
        {
            _instance = new SalesTaskPad();
        }

        public static SalesTaskPad Instance
        {
            get { return _instance; }
        }

        public SalesTaskPad()
        {
            InitializeComponent();
            this.DataContextChanged += SalesTaskPad_DataContextChanged;

            gmap.MapProvider = GMapProviders.GoogleHybridMap;
            gmap.Position = new PointLatLng(12.053076334122, -61.7540377378464);
            gmap.Zoom = 13;

            SalesVM.Instance.PropertyChanged += InstanceOnPropertyChanged;

            currentMarker = new GMapMarker(gmap.Position);
            {
                currentMarker.Shape = new CustomMarkerRed(this, currentMarker, "custom position marker");
                currentMarker.Offset = new Point(-15, -15);
                currentMarker.ZIndex = int.MaxValue;
                gmap.Markers.Add(currentMarker);
            }
        }

        private void InstanceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "TransactionData")
            {
                if (SalesVM.Instance.TransactionData == null) return;
                currentMarker.Position = SalesVM.Instance.TransactionData.Position;
                gmap.CenterPosition = SalesVM.Instance.TransactionData.Position;

                SalesVM.Instance.TransactionData.PropertyChanged += TransactionData_PropertyChanged;
            }
        }

        private void TransactionData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            currentMarker.Position = SalesVM.Instance.TransactionData.Position;
            //gmap.CenterPosition = SalesVM.Instance.TransactionData.Position;
        }

        GMapMarker currentMarker;
        private void SalesLst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReBindItemEditor();

        }


        private void SalesTaskPad_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (SearchBox != null) SearchBox.Focus();
            ReBindTranStatusTxt();
        }

        private void ReBindTranStatusTxt()
        {
            Binding myBinding = new Binding("Status");
            myBinding.Source = SalesVM.Instance.TransactionData;
            if (SalesPad != null)
            {
                var frameworkElement = (FrameworkElement) SalesPad.FindName("TransStatusTxt");
                if (frameworkElement != null)
                    if (TextBlock.TextProperty != null) frameworkElement.SetBinding(TextBlock.TextProperty, myBinding);
            }
        }


        private void ReBindItemEditor()
        {
            try
            {
                ReBindTranStatusTxt();
                Binding myBinding = new Binding("SelectedItem");
                myBinding.Source = SalesView.SalesLst;
                myBinding.Mode = BindingMode.OneWay;

                if (ContentProperty != null)
                    ((FrameworkElement) SalesPad.FindName("ItemEditor")).SetBinding(ContentProperty, myBinding);
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }





        public TransactionBase Transaction
        {
            get { return SalesVM.Instance.TransactionData; }
        }


        private void SalesPad_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {

                if (e == null) return;
                var uie = e.OriginalSource as Control;

                if (uie == null) uie = SearchBox as Control;
                if (uie.Name == "PART_FilterBox")
                {
                    if (e.Key == Key.Enter) //pkey == Key.Enter &&
                    {
                        // SearchBox.RaiseFilterEvent();
                        //if (SearchListCtl.Items.Count == 1)
                        //    SearchListCtl.SelectedIndex = 0;
                        //var textBox = uie as TextBox;
                        //if (textBox != null) textBox.Text = "";
                        ////e.Handled = true;
                        //MoveToNextControl(uie);
                    }

                    return;
                }

                if (e.Key == Key.Enter)
                {
                    // e.Handled = true;
                   // MoveToNextControl(uie);
                }
                pkey = e.Key;
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public void MoveToNextControl(object sender)
        {
            try
            {
                UIElement uie = sender as UIElement;
                uie.MoveFocus(
                    new TraversalRequest(
                        FocusNavigationDirection.Next));
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private void FilterControl_Direction_1(object sender, DirectionEventArgs e)
        {
            try
            {
                if (e.Direction == DirectionEnum.Down)
                {
                    if (SearchListCtl != null) SearchListCtl.SelectedIndex += 1;
                }
                if (e.Direction == DirectionEnum.Up && SearchListCtl.SelectedIndex > -1)
                {
                    SearchListCtl.SelectedIndex -= 1;
                }
                if (e.Direction == DirectionEnum.Right)
                {

                    MoveToNextControl(sender);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private Key pkey;

        private void SearchBox_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Delete)
                {
                   // DeleteTransactionEntry();
                }



                if (e.Key == Key.Enter)
                {
                   // if (SalesVM.Instance != null) if (SearchBox != null) SalesVM.Instance.GetSearchResults(SearchBox.Text);
                    //if (SearchListCtl.Items.Count == 3)
                    //{
                    //    SearchListCtl.SelectedIndex = 2;
                    //    LocalProcesItem(SearchListCtl.SelectedItem);
                    //    MoveToNextControl(sender);
                    //    HideSearchList();
                    //}

                }


                pkey = e.Key;
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private void DeleteTransactionEntry()
        {
            try
            {
                if (SalesVM.Instance.TransactionData == null)
                {
                    MessageBox.Show("Current Transaction Line not selected!");
                    return;
                }
                if (SalesVM.Instance.TransactionData.CurrentTransactionEntry == null)
                {
                    MessageBox.Show("Current Transaction Line not selected!");
                    var t = SalesVM.Instance.TransactionData.TransactionEntries.FirstOrDefault() as PrescriptionEntry;
                    if (t != null)
                        SalesVM.Instance.TransactionData.CurrentTransactionEntry = t;
                    return;
                }
                if (SalesVM.Instance.TransactionData != null &&
                    SalesVM.Instance.TransactionData.CurrentTransactionEntry != null)
                {
                    SalesVM.Instance.DeleteTransactionEntry<TransactionEntryBase>(
                        SalesVM.Instance.TransactionData.CurrentTransactionEntry);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public void LocalProcesItem(object itm)
        {
            try
            {
                if (ItemEditor == null) return;
                if (itm == null) return;

                if (edititem == true)
                {
                    ItemEditor.Content = itm;
                    edititem = false;
                    return;
                }


                if (itm.GetType() == typeof (RMSDataAccessLayer.SearchItem))
                {
                    switch (((ISearchItem) itm).DisplayName)
                    {

                        case "Add Patient":


                            Patient p = SalesVM.Instance.CreateNewPatient(SearchBox.Text);
                            p.StartTracking();
                            ItemEditor.Content = p;

                            break;
                        case "Add Doctor":
                            Doctor d = SalesVM.Instance.CreateNewDoctor();
                            d.StartTracking();
                            ItemEditor.Content = d;
                            break;

                        case "Add Drug":
                            Medicine i = new Medicine() { TrackingState = TrackingState.Added };
                            i.StartTracking();
                            if (!string.IsNullOrEmpty(SearchBox.Text)) i.ItemName = SearchBox.Text;
                            ItemEditor.Content = i;
                            break;

                        default:
                            ItemEditor.Content = ((RMSDataAccessLayer.SearchItem) itm).SearchObject;
                            break;
                    }
                }



                else
                {
                    if (showPatientTransactions == true)
                    {
                        if (itm.GetType() == typeof (RMSDataAccessLayer.Patient))
                        {
                            var lst = SalesVM.Instance.GetPatientTransactionList(itm as Patient);
                            ItemEditor.Content = lst;
                            showPatientTransactions = false;
                        }

                        if (itm.GetType() == typeof (RMSDataAccessLayer.Doctor))
                        {
                            var lst = SalesVM.Instance.GetDoctorTransactionList(itm as Doctor);
                            ItemEditor.Content = lst;
                            showPatientTransactions = false;
                        }
                    }
                    else
                    {
                        if (SalesVM.Instance != null) SalesVM.Instance.ProcessSearchListItem(itm);
                        MoveToNextControl(ItemEditor);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }


        private SalesView _SalesView;

        public SalesView SalesView
        {
            get { return _SalesView; }
            set
            {
                _SalesView = value;
                SalesView.SalesLst.SelectionChanged += SalesLst_SelectionChanged;
            }
        }



        private Timer queryTimer;

        private void RevokeQueryTimer()
        {
            if (queryTimer != null)
            {
                queryTimer.Stop();
                queryTimer.Elapsed -= queryTimer_Tick;
                queryTimer = null;
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(HideSearchList));
            }
        }

        private void RestartQueryTimer()
        {
            // Start or reset a pending query
            if (queryTimer == null)
            {
                queryTimer = new Timer {Enabled = true, Interval = 500};
                queryTimer.Elapsed += queryTimer_Tick;
            }
            else
            {
                queryTimer.Stop();
                queryTimer.Start();
            }
        }

        private void queryTimer_Tick(object sender, EventArgs e)
        {
            // Stop the timer so it doesn't fire again unless rescheduled
            RevokeQueryTimer();

            // Perform the query
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                
                if (SalesVM.Instance != null) if (SearchBox != null) SalesVM.Instance.GetSearchResults(SearchBox.Text);
                ShowSearchList();
            }));
        }

        private void SearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e != null && e.Changes != null)
                {
                    
                    //if (SalesVM.Instance != null) if (SearchBox != null) SalesVM.Instance.GetSearchResults(SearchBox.Text);

                    //ShowSearchList();
                    RestartQueryTimer();
                  // ShowSearchList();
                }
                else
                {
                    HideSearchList();
                 //   RevokeQueryTimer();
                }

            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }


        private void HideSearchList()
        {
            if (SearchListCtl != null)
            {
                SearchListCtl.Visibility = System.Windows.Visibility.Collapsed;
                SearchListCtl.Focusable = false;
                SearchListCtl.SelectedIndex = -1;
            }
        }

        private void ShowSearchList()
        {
            if (SearchListCtl != null && !string.IsNullOrEmpty(SearchBox.Text))
            {
                SearchListCtl.Visibility = System.Windows.Visibility.Visible;
                SearchListCtl.Focusable = true;
                if (SearchListCtl.Items.Count == 1)
                    SearchListCtl.SelectedIndex = 0;
            }
        }



        private void TextBox_GotKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null) textBox.SelectAll();
        }

        public async void PrintBtn_Click_1(object sender, RoutedEventArgs e)
        {


            if (ReceiptCol != null && ReceiptCol.Width == new GridLength(0))
            {
                // unhide the colums to print
                ReceiptCol.Width = new GridLength(400);
                ReceiptGrd.UpdateLayout();

                await Print().ConfigureAwait(false);

                ReceiptCol.Width = new GridLength(0);
                //hide it back
            }
            else
            {

                await Print().ConfigureAwait(false);

            }

        }


        private async Task Print()
        {
            try
            {
                if (SalesVM.Instance != null &&
                    (SalesVM.Instance.TransactionData is QuickPrescription ||
                     SalesVM.Instance.TransactionData is Prescription))
                {
                    
                    ListView plist = Common.FindChild<ListView>(ReceiptGrd, "PrescriptionEntriesRptLst");
                    // (ListView)this.FindName("PrescriptionEntriesRptLst");
                    if (plist != null)
                        PrintListItems(plist);
                }

                //else
                //{
                //    if (SalesVM.Instance != null) SalesVM.Instance.Print(ref ReceiptGrd);
                //}
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error,
                    GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private void PrintListItems(ListView plist)
        {
            try
            {
                if (plist == null) return;
                dynamic lst;
                if (plist.SelectedItems.Count == 0)
                {
                    lst = plist.Items;
                }
                else
                {
                    lst = plist.SelectedItems;
                }

                foreach (var itm in lst)
                {

                    if (plist.ItemContainerGenerator != null)
                    {
                        var fi =
                            (FrameworkElement) plist.ItemContainerGenerator.ContainerFromItem(itm);
                        if (fi != null)
                        {
                            PrintEntry(fi, itm);
                        }
                        else
                        {
                            MessageBox.Show("Print error! Please check prints and reprint and also tell joseph you saw this error in TaskPad.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private void PrintEntry(FrameworkElement fi, PrescriptionEntry item)
        {
            Task.Run(() =>
            {

                Dispatcher.Invoke(DispatcherPriority.Send, new Action(() => { SalesVM.Instance.Print(ref fi, item); }));
            }).ConfigureAwait(false);
        }


        private void NewTransaction(object sender, RoutedEventArgs e)
        {
            if (SalesVM.Instance != null) SalesVM.Instance.TransactionData = SalesVM.Instance.NewPrescription();
        }

        private bool focusswitch;

        private void TransStatusTxt_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (focusswitch == true)
            {
                ReBindTranStatusTxt();
            }

            focusswitch = !focusswitch;

        }


        private int TemplateHeight = 192;

        private void PrescriptionEntriesRptLst_LayoutUpdated(object sender, EventArgs e)
        {
            //if (SalesView != null &&
            //    (SalesVM.Instance != null &&
            //     (SalesVM.Instance.TransactionData != null && SalesView.SalesPadState == SalesPadTransState.Receipt)))
            //    if (SalesVM.Instance.TransactionData.TransactionEntries != null)
            //    {
            //        var frameworkElement = (FrameworkElement) this.FindName("ReceiptGrd");
            //        if (frameworkElement != null)
            //            frameworkElement.Height = TemplateHeight*
            //                                      SalesVM.Instance.TransactionData.TransactionEntries.Count;
            //    }
        }


        private void SearchListCtl_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            if (SearchListCtl != null) LocalProcesItem(SearchListCtl.SelectedItem);
            MoveToNextControl(sender);
            if (SearchBox != null) SearchBox.Text = "";
            HideSearchList();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SearchListCtl != null && SearchListCtl.Visibility == System.Windows.Visibility.Visible)
            {
                HideSearchList();
            }
            else
            {
                ShowSearchList();
            }
        }

        private void NextEntry_Click(object sender, RoutedEventArgs e)
        {
            if (SalesView != null)
            {
                SalesView.pkey = Key.Down;
                SalesView.padPos = SalesRegion.SalesView.PadPosition.Middle;
                SalesView.GoToNextTransactionEntry();
            }
        }

        private void DeleteTranBtn_Click(object sender, RoutedEventArgs e)
        {
            DeleteTransactionEntry();
        }

        private void BackBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SalesView != null) SalesView.GotoPreviousSalesStep();
        }

        private void NextBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SalesView != null) SalesView.GotoNextSalesStep(Key.Right);
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (SalesVM.Instance != null)
                {
                    var transactionBase = e.AddedItems[0] as TransactionBase;
                    if (transactionBase != null)
                        SalesVM.Instance.GoToTransaction(transactionBase.TransactionId);

                }
                ReBindItemEditor();
            }
        }

        private void SavePatientBtn_Click(object sender, RoutedEventArgs e)
        {

            if (ItemEditor.Content is Person)
            {
               if(!SalesVM.Instance.SavePerson((Person) ItemEditor.Content)) return;
            }
            
            LocalProcesItem(ItemEditor.Content);
            ReBindItemEditor();
        }

        private void SaveMedicineBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ItemEditor.Content is Medicine m)
                if (!SalesVM.Instance.SaveMedicine((Medicine) ItemEditor.Content)) return;
            LocalProcesItem(ItemEditor.Content);
            ReBindItemEditor();
        }

        


        private bool edititem = false;

        public void EditItemTB_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            edititem = true;
        }

        private bool showPatientTransactions = false;

        private void PatientPrescriptionBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            showPatientTransactions = true;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ItemEditor.Content = null;
            ReBindItemEditor();
            // SalesVM.Instance.DiscardChanges();
        }

        private async void PostToQB_Click(object sender, RoutedEventArgs e)
        {
           await Task.Run(() => SalesVM.Instance.PostQBSale()).ConfigureAwait(false);
            UIDispatcher.Current.BeginInvoke(() =>
            {
                NextBtn_MouseLeftButtonDown(sender, null);
            });
            
        }



        private async void PrintBtn_RightClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ListView plist = Common.FindChild<ListView>(ReceiptGrd, "PrescriptionEntriesRptLst");
                // (ListView)this.FindName("PrescriptionEntriesRptLst");
                if (plist != null)
                {
                    plist.SelectAll();
                    PrintBtn_Click_1(sender, e);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListView plist = Common.FindChild<ListView>(ReceiptGrd, "PrescriptionEntriesRptLst");
            // (ListView)this.FindName("PrescriptionEntriesRptLst");
            if (plist != null)
            {
                plist.SelectedItems.Clear();
            }
        }

        private void PharmacistCbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e != null && e.AddedItems.Count > 0)
                {
                    Cashier c = (e.AddedItems[0] as Cashier);
                    if (c != null && SalesVM.Instance.TransactionData != null)
                        SalesVM.Instance.TransactionData.PharmacistId = c.Id;
                    SalesVM.Instance.SaveTransaction();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private void SetDosage(object sender, MouseButtonEventArgs e)
        {
           
            if (SalesVM.Instance.TransactionData.CurrentTransactionEntry.TransactionEntryItem != null)
            {
                using (var ctx = new RMSModel())
                {
                    var m =
                        ctx.Item.FirstOrDefault(
                            x =>
                                x.ItemNumber ==
                                SalesVM.Instance.TransactionData.CurrentTransactionEntry.TransactionEntryItem.ItemNumber);
                    m.ItemPresetDosages.Add(new ItemPresetDosage()
                    {
                        Dosage = SalesVM.Instance.TransactionData.CurrentTransactionEntry.Dosage,
                        ItemId = m.ItemId
                    });
                    ctx.Item.AddOrUpdate(m);
                    ctx.SaveChanges();
                    MessageBox.Show("Pre-Dosage Saved");
                }

                //using (var ctx = new RMSModel())
                //{
                //    var m =
                //        ctx.Item.OfType<Medicine>().FirstOrDefault(
                //            x =>
                //                x.ItemId ==
                //                SalesVM.Instance.TransactionData.CurrentTransactionEntry.TransactionEntryItem.ItemId);
                //    m.SuggestedDosage = SalesVM.Instance.TransactionData.CurrentTransactionEntry.Dosage;
                //    ctx.Item.AddOrUpdate(m);
                //    ctx.SaveChanges();
                //    MessageBox.Show("Suggested Dosage Saved");
                //}
            }
        }

        private void presetDosageChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e != null && e.AddedItems.Count > 0)
            {
                var c = (e.AddedItems[0] as PresetDosage);
                SalesVM.Instance.TransactionData.CurrentTransactionEntry.Dosage += $" {c.Dosage}";
            }
        }

        private void AddDosageCode(object sender, MouseButtonEventArgs e)
        {
            ComboBox box = Common.FindChild<ComboBox>(TransactionGrd, "DosageCodes");
            AutoCompleteTextBox dosage = Common.FindChild<AutoCompleteTextBox>(TransactionGrd, "AutoCompleteTextBox");
            if (box == null || dosage == null) return;
            using (var ctx = new RMSModel())
            {
                var m =
                    ctx.RxAbbrevations.FirstOrDefault(
                        x =>
                            x.Shortcut == box.Text) ?? new RxAbbrevation() { Shortcut = box.Text, Sentence = dosage.Text };
                ctx.RxAbbrevations.AddOrUpdate(m);
                ctx.SaveChanges();
                SalesVM.Instance.LoadRxAbbrevations();
                MessageBox.Show("Dosage & Code Saved");
            }
        }

        private void DosageCodes_SelectionChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            ComboBox box = Common.FindChild<ComboBox>(TransactionGrd, "DosageCodes");
            AutoCompleteTextBox dosage = Common.FindChild<AutoCompleteTextBox>(TransactionGrd, "AutoCompleteTextBox");
            if (dosage == null || SalesVM.Instance.SelectedRxAbbrevation == null) return;

            dosage.Text = SalesVM.Instance.SelectedRxAbbrevation.Sentence;
            // 
        }


        private void DosageCodes_OnTextInput(object sender, KeyEventArgs keyEventArgs)
        {

            //if (((int) keyEventArgs.Key >= (int) Key.A && (int) keyEventArgs.Key <= (int) Key.Z)
            //    || ((int) keyEventArgs.Key >= (int) Key.NumPad0 && (int) keyEventArgs.Key <= (int) Key.NumPad9)
            //    || ((int) keyEventArgs.Key >= (int) Key.D0 && (int) keyEventArgs.Key <= (int) Key.D9)
            //    || (int)keyEventArgs.Key == (int)Key.Space
            //    || (int)keyEventArgs.Key == (int)Key.Divide
            //    || (int)keyEventArgs.Key == (int)Key.Escape)
            //{
            //    var res = keyEventArgs.Key.ToString().Replace("NumPad","").Replace("Divide","/").Replace("D","").Replace("Space"," ");
            //    SalesVM.Instance.RxSearchText += res;
            //    if (keyEventArgs.Key == Key.Escape) SalesVM.Instance.RxSearchText = "";
            //SalesVM.Instance.LoadRxAbbrevations(SalesVM.Instance.RxSearchText);
            //}

        }


        private void Gmap_OnOnPositionChanged(PointLatLng point)
        {
           // MapGroup.Header = "gmap: " + point;
            //SalesVM.Instance.TransactionData.TransactionLocation = new TransactionLocation(){Longitude = point.Lng, Latitude = point.Lat};
            //SalesVM.Instance.SaveTransaction();
        }

        private void PickUp_Click(object sender, RoutedEventArgs e)
        {
            SalesVM.Instance.TransactionData.DeliveryType = "Pickup";
            SalesVM.Instance.TransactionData.TransactionLocation = null;
            SalesVM.Instance.SaveTransaction();

            PrintBtn_Click_1(null, null);

            Task.Run(() =>
            {

                Dispatcher.Invoke(DispatcherPriority.Send, new Action(() =>
                {
                    var frm = ((FrameworkElement)OrderLabel);
                    SalesVM.Instance.Print(ref frm);
                }));
            }).ConfigureAwait(false);
        }

        private void Delivery_Click(object sender, RoutedEventArgs e)
        {
            SalesVM.Instance.TransactionData.DeliveryType = "Delivery";
            SalesVM.Instance.SaveTransaction();

            PrintBtn_Click_1(null, null);

            Task.Run(() =>
            {

                Dispatcher.Invoke(DispatcherPriority.Send, new Action(() =>
                {
                    var frm = ((FrameworkElement)OrderLabel);
                    SalesVM.Instance.Print(ref frm);
                }));
            }).ConfigureAwait(false);
            
        }

        private void Gmap_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var p = e.GetPosition(gmap);
            currentMarker.Position = gmap.FromLocalToLatLng((int)p.X, (int)p.Y);
            if(SalesVM.Instance.TransactionData != null) SalesVM.Instance.TransactionData.Position = currentMarker.Position;
        }

        private void CallinBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            SalesVM.Instance.TransactionData.DeliveryType = "Call-In Rx";
            SalesVM.Instance.SaveTransaction();

            PrintBtn_Click_1(null, null);

            Task.Run(() =>
            {

                Dispatcher.Invoke(DispatcherPriority.Send, new Action(() =>
                {
                    var frm = ((FrameworkElement)OrderLabel);
                    SalesVM.Instance.Print(ref frm);
                    SalesVM.Instance.Transaction2Pdf(ref PrescriptionPad);
                }));
            }).ConfigureAwait(false);
        }

        private void ScanBtn_Click
            (object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddPicture(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var patient = ((dynamic) SalesVM.Instance.TransactionData).Patient;
                if (patient == null)
                {
                    MessageBox.Show("Please add Patient before selecting Picture.");
                    return;
                }

                FileDialog fldlg = new OpenFileDialog();
                fldlg.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                fldlg.Filter = "Image File (*.jpg;*.bmp;*.gif)|*.jpg;*.bmp;*.gif";
                fldlg.ShowDialog();
                {

                    var imageName = fldlg.FileName;
                    if (string.IsNullOrEmpty(imageName)) return;
                    //ImageSourceConverter isc = new ImageSourceConverter();
                    //Photo.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));
                    e.Handled = true;
                    SalesVM.Instance.SavePhoto(patient, imageName);

                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void AddPrescriptionPhoto(object sender, MouseButtonEventArgs e)
        {
            try
            {
               
                if (!(SalesVM.Instance.TransactionData is Prescription))
                {
                    MessageBox.Show("Only Prescriptions are allowed.");
                    return;
                }

                SalesVM.Instance.SaveTransaction(SalesVM.Instance.TransactionData);
                FileDialog fldlg = new OpenFileDialog();
                fldlg.InitialDirectory = SalesVM.Instance.PrescriptionPhotoFolder;//Environment.SpecialFolder.MyDocuments.ToString();
                fldlg.Filter = "Image File (*.jpg;*.bmp;*.gif)|*.jpg;*.bmp;*.gif";
                fldlg.ShowDialog();
                {

                    var imageName = fldlg.FileName;
                    if (string.IsNullOrEmpty(imageName)) return;
                    //ImageSourceConverter isc = new ImageSourceConverter();
                    //Photo.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));
                    e.Handled = true;
                    SalesVM.Instance.SavePrescriptionPhoto(SalesVM.Instance.TransactionData, imageName);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private static FileInfo CurrentPhotoPath = null;
        private void NextPhoto_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var files = new DirectoryInfo(SalesVM.Instance.PrescriptionPhotoFolder).GetFiles("*.jpg",
                SearchOption.TopDirectoryOnly).Where(x => x.CreationTime > (CurrentPhotoPath?.CreationTime??DateTime.Today)).OrderBy(x => x.CreationTime);
            if (!files.Any()) return;
            CurrentPhotoPath = files.FirstOrDefault();
            if (SalesVM.Instance.TransactionData == null)
                SalesVM.Instance.TransactionData = new Prescription() { PrescriptionImage = new PrescriptionImage()};
            if(CurrentPhotoPath != null )  SalesVM.Instance.SavePrescriptionPhoto(SalesVM.Instance.TransactionData, CurrentPhotoPath.FullName);

            

        }

        private void BackPhoto_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var files = new DirectoryInfo(SalesVM.Instance.PrescriptionPhotoFolder).GetFiles("*.jpg",
                SearchOption.TopDirectoryOnly).Where(x => x.CreationTime > DateTime.Today && x.CreationTime < (CurrentPhotoPath?.CreationTime ?? DateTime.Today)).OrderByDescending(x => x.CreationTime);
            if (!files.Any()) return;
            CurrentPhotoPath = files.FirstOrDefault();
            if (SalesVM.Instance.TransactionData == null)
                SalesVM.Instance.TransactionData = new Prescription() { PrescriptionImage = new PrescriptionImage() };
            if (CurrentPhotoPath != null) SalesVM.Instance.SavePrescriptionPhoto(SalesVM.Instance.TransactionData, CurrentPhotoPath.FullName);
        }

        private Patient walkinPatient = null;
        private void AddWalkinPatient(object sender, RoutedEventArgs e)
        {
            if (walkinPatient == null)
            {
                using (var ctx = new RMSModel())
                {
                    this.walkinPatient =
                        ctx.Persons.OfType<Patient>().FirstOrDefault(
                            x =>
                                x.FirstName.Trim().ToUpper() == "Walk in".ToUpper());
                }
            }
            if (SalesVM.Instance.TransactionData == null) SalesVM.Instance.TransactionData = SalesVM.Instance.NewPrescription();
            ((Prescription)SalesVM.Instance.TransactionData).Patient = walkinPatient;
            ((Prescription)SalesVM.Instance.TransactionData).PatientId = walkinPatient.Id;
        }
    }


}
