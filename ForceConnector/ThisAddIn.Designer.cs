﻿// ------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// Runtime Version:4.0.30319.42000
// 
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

using System;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;

namespace ForceConnector
{



    /// 
    [Microsoft.VisualStudio.Tools.Applications.Runtime.StartupObject(0)]
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public sealed partial class ThisAddIn : Microsoft.Office.Tools.AddInBase
    {

        /// 
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public ThisAddIn(ApplicationFactory factory, IServiceProvider serviceProvider) : base(factory, serviceProvider, "AddIn", "ThisAddIn")
        {
            this.Startup += (_, __) => ThisAddIn_Startup();
            this.Shutdown += (_, __) => ThisAddIn_Shutdown();
            Globals.Factory = factory;
        }

        internal Microsoft.Office.Tools.CustomTaskPaneCollection CustomTaskPanes;
        internal Microsoft.Office.Tools.SmartTagCollection VstoSmartTags;
        [System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        internal Excel.Application Application;

        /// 
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void Initialize()
        {
            base.Initialize();
            Application = GetHostItem<Excel.Application>(typeof(Excel.Application), "Application");
            Globals.ThisAddIn = this;
            System.Windows.Forms.Application.EnableVisualStyles();
            InitializeCachedData();
            InitializeControls();
            InitializeComponents();
            InitializeData();
        }

        /// 
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void FinishInitialization()
        {
            OnStartup();
        }

        /// 
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void InitializeDataBindings()
        {
            BeginInitialization();
            BindToData();
            EndInitialization();
        }

        /// 
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private void InitializeCachedData()
        {
            if (DataHost is null)
            {
                return;
            }

            if (DataHost.IsCacheInitialized)
            {
                DataHost.FillCachedData(this);
            }
        }

        /// 
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private void InitializeData()
        {
        }

        /// 
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private void BindToData()
        {
        }

        /// 
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        private void StartCaching(string MemberName)
        {
            DataHost.StartCaching(this, MemberName);
        }

        /// 
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        private void StopCaching(string MemberName)
        {
            DataHost.StopCaching(this, MemberName);
        }

        /// 
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        private bool IsCached(string MemberName)
        {
            return DataHost.IsCached(this, MemberName);
        }

        /// 
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private void BeginInitialization()
        {
            BeginInit();
            CustomTaskPanes.BeginInit();
            VstoSmartTags.BeginInit();
        }

        /// 
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private void EndInitialization()
        {
            VstoSmartTags.EndInit();
            CustomTaskPanes.EndInit();
            EndInit();
        }

        /// 
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private void InitializeControls()
        {
            CustomTaskPanes = Globals.Factory.CreateCustomTaskPaneCollection(null, null, "CustomTaskPanes", "CustomTaskPanes", this);
            VstoSmartTags = Globals.Factory.CreateSmartTagCollection(null, null, "VstoSmartTags", "VstoSmartTags", this);
        }

        /// 
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private void InitializeComponents()
        {
        }

        /// 
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        private bool NeedsFill(string MemberName)
        {
            return DataHost.NeedsFill(this, MemberName);
        }

        /// 
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void OnShutdown()
        {
            VstoSmartTags.Dispose();
            CustomTaskPanes.Dispose();
            base.OnShutdown();
        }
    }

    /// 
    [System.Diagnostics.DebuggerNonUserCode()]
    [System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
    internal sealed partial class Globals
    {

        /// 
        private Globals() : base()
        {
        }

        private static ThisAddIn _ThisAddIn;
        private static ApplicationFactory _factory;
        private static ThisRibbonCollection _ThisRibbonCollection;

        internal static ThisAddIn ThisAddIn
        {
            get
            {
                return _ThisAddIn;
            }

            set
            {
                if (_ThisAddIn is null)
                {
                    _ThisAddIn = value;
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        internal static ApplicationFactory Factory
        {
            get
            {
                return _factory;
            }

            set
            {
                if (_factory is null)
                {
                    _factory = value;
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        internal static ThisRibbonCollection Ribbons
        {
            get
            {
                if (_ThisRibbonCollection is null)
                {
                    _ThisRibbonCollection = new ThisRibbonCollection(_factory.GetRibbonFactory());
                }

                return _ThisRibbonCollection;
            }
        }
    }

    /// 
    [System.Diagnostics.DebuggerNonUserCode()]
    [System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
    internal sealed partial class ThisRibbonCollection : Microsoft.Office.Tools.Ribbon.RibbonCollectionBase
    {

        /// 
        internal ThisRibbonCollection(Microsoft.Office.Tools.Ribbon.RibbonFactory factory) : base(factory)
        {
        }
    }
}