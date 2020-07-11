﻿using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System;

namespace OrkadWeb.Models.Enums.NHibernate
{
    public class EnumConvention : IPropertyConvention, IPropertyConventionAcceptance
    {
        #region IPropertyConvention Members

        public void Apply(IPropertyInstance instance)
        {
            instance.CustomType(instance.Property.PropertyType);
        }

        #endregion

        #region IPropertyConventionAcceptance Members

        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Property.PropertyType.IsEnum ||
            (x.Property.PropertyType.IsGenericType &&
             x.Property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
             x.Property.PropertyType.GetGenericArguments()[0].IsEnum)
            );
        }

        #endregion
    }
}
