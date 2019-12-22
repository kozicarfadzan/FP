﻿using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Data.EntityModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Helper
{
    public static class DropdownHelper
    {
        public static IEnumerable<SelectListItem> GetEnumSelectList<T>()
        {
            return (Enum.GetValues(typeof(T)).Cast<int>().Select(e => new SelectListItem() { Text = Enum.GetName(typeof(T), e), Value = e.ToString() })).ToList();
        }


        public static IEnumerable<SelectListItem> GetEntitySelectList<TEntity>(this HttpContext context, string value, string text) where TEntity: class
        {
            MyContext db = context.RequestServices.GetService(typeof(MyContext)) as MyContext;

            foreach (var dbset in db.GetType().GetProperties())
            {
                if(dbset.PropertyType == typeof(DbSet<TEntity>))
                {
                    Type type = dbset.PropertyType;
                    List<TEntity> list = ((DbSet<TEntity>)dbset.GetValue(db)).ToList();
                    SelectList selectlist = new SelectList(list, value, text);

                    return selectlist.ToList();
                }

            }

            return null;

        }
    }
}
