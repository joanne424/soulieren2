﻿#pragma checksum "..\..\..\..\Chart\ChartWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B7445F4FDE30255CC4EF25534CB16250"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using ActiproSoftware.Products.Docking;
using ActiproSoftware.Windows.Controls.Docking;
using ActiproSoftware.Windows.Controls.Docking.Preview;
using ActiproSoftware.Windows.Controls.Docking.Primitives;
using ActiproSoftware.Windows.Controls.Docking.Switching;
using DM2.Ent.Client.Views;
using ModulusFE;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace DM2.Ent.Client.Views {
    
    
    /// <summary>
    /// ChartWindow
    /// </summary>
    public partial class ChartWindow : ActiproSoftware.Windows.Controls.Docking.DocumentWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\Chart\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ModulusFE.StockChartX stockChartX;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/DM2.Ent.Client.Views;component/chart/chartwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Chart\ChartWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.stockChartX = ((ModulusFE.StockChartX)(target));
            
            #line 27 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.CreateOrderClick += new System.EventHandler<ModulusFE.StockChartX.CreateOrderClickEventArgs>(this.StockChartX_CreateOrderClick);
            
            #line default
            #line hidden
            
            #line 28 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.ImageAsSaveClick += new System.EventHandler(this.StockChartX_ImageAsSaveClick);
            
            #line default
            #line hidden
            
            #line 29 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.ImageAsPrintClick += new System.EventHandler(this.StockChartX_ImageAsPrintClick);
            
            #line default
            #line hidden
            
            #line 30 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.SeriesRightClick += new System.EventHandler<ModulusFE.StockChartX.SeriesRightClickEventArgs>(this.StockChartX_SeriesRightClick);
            
            #line default
            #line hidden
            
            #line 31 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.SeriesBeforeDelete += new System.EventHandler<ModulusFE.StockChartX.SeriesBeforeDeleteEventArgs>(this.StockChartX_SeriesBeforeDelete);
            
            #line default
            #line hidden
            
            #line 32 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.ResetChartHandler += new System.EventHandler<ModulusFE.StockChartX.ResetChartEventArgs>(this.StockChartX_ResetChartHandler);
            
            #line default
            #line hidden
            
            #line 33 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.CloseCurrentPage += new System.EventHandler(this.StockChartX_CloseCurrentPage);
            
            #line default
            #line hidden
            
            #line 34 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.ChartGetHistoryPrice += new System.EventHandler<ModulusFE.StockChartX.ChartGetHistoryPriceEventArgs>(this.StockChartX_ChartGetHistoryPrice);
            
            #line default
            #line hidden
            
            #line 35 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.LineStudyDoubleClick += new System.EventHandler<ModulusFE.StockChartX.LineStudyMouseEventArgs>(this.StockChartX_LineStudyDoubleClick);
            
            #line default
            #line hidden
            
            #line 36 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.ChartStyleColor += new System.EventHandler(this.StockChartX_ChartStyleColor);
            
            #line default
            #line hidden
            
            #line 37 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.TemplateHandle += new System.EventHandler<ModulusFE.StockChartX.TemplateHandleEventArgs>(this.StockChartX_TemplateHandle);
            
            #line default
            #line hidden
            
            #line 38 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.LayoutHandle += new System.EventHandler<ModulusFE.StockChartX.LayoutHandleEventArgs>(this.StockChartX_LayoutHandle);
            
            #line default
            #line hidden
            
            #line 39 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.ChartLoaded += new System.EventHandler(this.StockChartX_ChartLoaded);
            
            #line default
            #line hidden
            
            #line 40 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.SizeChanged += new System.Windows.SizeChangedEventHandler(this.StockChartX_SizeChanged);
            
            #line default
            #line hidden
            
            #line 41 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.ShortcutLayoutLoaded += new System.EventHandler<ModulusFE.StockChartX.ShortcutLayoutEventArgs>(this.StockChartX_ShortcutLayoutLoaded);
            
            #line default
            #line hidden
            
            #line 42 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.ObjectManage += new System.EventHandler(this.StockChartX_ObjectManage);
            
            #line default
            #line hidden
            
            #line 43 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.TurnChart += new System.EventHandler<ModulusFE.StockChartX.TurnChartEventArgs>(this.StockChartX_TurnChart);
            
            #line default
            #line hidden
            
            #line 44 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.TurnToChartDeleteObjectManage += new System.EventHandler(this.StockChartX_TurnToChartDeleteObjectManage);
            
            #line default
            #line hidden
            
            #line 45 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.OpenSymbolList += new System.EventHandler(this.StockChartX_OpenSymbolList);
            
            #line default
            #line hidden
            
            #line 46 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.ToolBarMouseDown += new System.EventHandler(this.StockChartX_ToolBarMouseDown);
            
            #line default
            #line hidden
            
            #line 47 "..\..\..\..\Chart\ChartWindow.xaml"
            this.stockChartX.ToolBarCustomised += new System.EventHandler<ModulusFE.StockChartX.ToolBarCustomisedEventArgs>(this.StockChartX_ToolBarCustomised);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

