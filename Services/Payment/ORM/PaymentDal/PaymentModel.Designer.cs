﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]

namespace TicketingOffice.PaymentService.DataAccess
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class TicketingOfficePaymentEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new TicketingOfficePaymentEntities object using the connection string found in the 'TicketingOfficePaymentEntities' section of the application configuration file.
        /// </summary>
        public TicketingOfficePaymentEntities() : base("name=TicketingOfficePaymentEntities", "TicketingOfficePaymentEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new TicketingOfficePaymentEntities object.
        /// </summary>
        public TicketingOfficePaymentEntities(string connectionString) : base(connectionString, "TicketingOfficePaymentEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new TicketingOfficePaymentEntities object.
        /// </summary>
        public TicketingOfficePaymentEntities(EntityConnection connection) : base(connection, "TicketingOfficePaymentEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Payment> Payments
        {
            get
            {
                if ((_Payments == null))
                {
                    _Payments = base.CreateObjectSet<Payment>("Payments");
                }
                return _Payments;
            }
        }
        private ObjectSet<Payment> _Payments;

        #endregion
        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Payments EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToPayments(Payment payment)
        {
            base.AddObject("Payments", payment);
        }

        #endregion
    }
    

    #endregion
    
    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="TicketingOfficeModel", Name="Payment")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Payment : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Payment object.
        /// </summary>
        /// <param name="id">Initial value of the ID property.</param>
        /// <param name="amount">Initial value of the Amount property.</param>
        /// <param name="date">Initial value of the Date property.</param>
        /// <param name="methodOfPayment">Initial value of the MethodOfPayment property.</param>
        /// <param name="orderID">Initial value of the OrderID property.</param>
        /// <param name="customerID">Initial value of the CustomerID property.</param>
        public static Payment CreatePayment(global::System.Int64 id, global::System.Int32 amount, global::System.DateTime date, global::System.Int32 methodOfPayment, global::System.Guid orderID, global::System.Guid customerID)
        {
            Payment payment = new Payment();
            payment.ID = id;
            payment.Amount = amount;
            payment.Date = date;
            payment.MethodOfPayment = methodOfPayment;
            payment.OrderID = orderID;
            payment.CustomerID = customerID;
            return payment;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    OnIDChanging(value);
                    ReportPropertyChanging("ID");
                    _ID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("ID");
                    OnIDChanged();
                }
            }
        }
        private global::System.Int64 _ID;
        partial void OnIDChanging(global::System.Int64 value);
        partial void OnIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                OnAmountChanging(value);
                ReportPropertyChanging("Amount");
                _Amount = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Amount");
                OnAmountChanged();
            }
        }
        private global::System.Int32 _Amount;
        partial void OnAmountChanging(global::System.Int32 value);
        partial void OnAmountChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime Date
        {
            get
            {
                return _Date;
            }
            set
            {
                OnDateChanging(value);
                ReportPropertyChanging("Date");
                _Date = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Date");
                OnDateChanged();
            }
        }
        private global::System.DateTime _Date;
        partial void OnDateChanging(global::System.DateTime value);
        partial void OnDateChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 MethodOfPayment
        {
            get
            {
                return _MethodOfPayment;
            }
            set
            {
                OnMethodOfPaymentChanging(value);
                ReportPropertyChanging("MethodOfPayment");
                _MethodOfPayment = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("MethodOfPayment");
                OnMethodOfPaymentChanged();
            }
        }
        private global::System.Int32 _MethodOfPayment;
        partial void OnMethodOfPaymentChanging(global::System.Int32 value);
        partial void OnMethodOfPaymentChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid OrderID
        {
            get
            {
                return _OrderID;
            }
            set
            {
                OnOrderIDChanging(value);
                ReportPropertyChanging("OrderID");
                _OrderID = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("OrderID");
                OnOrderIDChanged();
            }
        }
        private global::System.Guid _OrderID;
        partial void OnOrderIDChanging(global::System.Guid value);
        partial void OnOrderIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid CustomerID
        {
            get
            {
                return _CustomerID;
            }
            set
            {
                OnCustomerIDChanging(value);
                ReportPropertyChanging("CustomerID");
                _CustomerID = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("CustomerID");
                OnCustomerIDChanged();
            }
        }
        private global::System.Guid _CustomerID;
        partial void OnCustomerIDChanging(global::System.Guid value);
        partial void OnCustomerIDChanged();

        #endregion
    
    }

    #endregion
    
}