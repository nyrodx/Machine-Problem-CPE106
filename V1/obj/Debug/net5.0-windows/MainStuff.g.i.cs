﻿#pragma checksum "..\..\..\MainStuff.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B9BAE5D8A637F535A4F712D6E8E69413AD89B382"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
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
using V1;


namespace V1 {
    
    
    /// <summary>
    /// MainStuff
    /// </summary>
    public partial class MainStuff : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 42 "..\..\..\MainStuff.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid students_grid;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\MainStuff.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid sideGrid;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\MainStuff.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_LoadSQL;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\MainStuff.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_LoadExcel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.15.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/V1;component/mainstuff.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainStuff.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.15.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\MainStuff.xaml"
            ((V1.MainStuff)(target)).Closed += new System.EventHandler(this.mainstuff_Close);
            
            #line default
            #line hidden
            return;
            case 2:
            this.students_grid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 48 "..\..\..\MainStuff.xaml"
            this.students_grid.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.students_grid_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.sideGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.btn_LoadSQL = ((System.Windows.Controls.Button)(target));
            
            #line 66 "..\..\..\MainStuff.xaml"
            this.btn_LoadSQL.Click += new System.Windows.RoutedEventHandler(this.btn_LoadSQL_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btn_LoadExcel = ((System.Windows.Controls.Button)(target));
            
            #line 69 "..\..\..\MainStuff.xaml"
            this.btn_LoadExcel.Click += new System.Windows.RoutedEventHandler(this.btn_LoadExcel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
