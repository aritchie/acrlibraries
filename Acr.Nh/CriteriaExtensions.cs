using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Acr.Collections;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.Transform;


namespace Acr.Nh {

    public static class CriteriaExtensions {

        public static bool HasAssociationRequest(this ICriteria criteria, string associationPath) {
            return ((CriteriaImpl)criteria).IterateSubcriteria().Any(x => x.Path == associationPath);
        }


        public static bool HasAlias(this ICriteria criteria, string alias) {
            return ((CriteriaImpl)criteria).IterateSubcriteria().Any(x => x.Alias == alias);
        }


        public static IList<T> ProjectionList<T>(this ICriteria criteria) {
            return criteria
                .SetResultTransformer(Transformers.AliasToBean<T>())
                .List<T>();
        }


        public static DataPage<T> ProjectionDataPage<T>(this ICriteria criteria, Pager pager) where T : class {
            return criteria
                .SetResultTransformer(Transformers.AliasToBean<T>())
                .DataPage<T>(pager);
        }


        public static ICriteria SetPage(this ICriteria criteria, int page, int maxResults) {
            page--;
            if (page < 0) 
                page = 0;
                
            var startRowIndex = page * maxResults;
            return criteria
                .SetFirstResult(startRowIndex)
                .SetMaxResults(maxResults);
        }


        public static DataPage<T> DataPage<T>(this ICriteria criteria, Pager pager) where T : class {
            if (pager.UsePages) {
                criteria.SetPage(pager.Start, pager.MaxResults);
            }
            else {
                criteria
                    .SetFirstResult(pager.Start)
                    .SetMaxResults(pager.MaxResults);
            }
            pager.Sorts.Each(x => criteria.AddOrder(x));
            
            var count = criteria
                .RowCount()
                .FutureValue<int>();

            var list = criteria.Future<T>();

            return new DataPage<T> {
                TotalCount = count.Value,
                Data = list.ToArray()
            };
        }


        public static ICriteria And(this ICriteria criteria, ICriterion cri1, ICriterion cri2) {
            return criteria.Add(Restrictions.And(cri1, cri2));
        }


        public static ICriteria Eq(this ICriteria criteria, string propertyName, object value) {
            return criteria.Add(Restrictions.Eq(propertyName, value));
        }


        public static ICriteria EqProperty(this ICriteria criteria, string propertyName1, string propertyName2) {
            return criteria.Add(Restrictions.EqProperty(propertyName1, propertyName2));
        }


        public static ICriteria Between(this ICriteria criteria, string propertyName, object lo, object hi) {
            return criteria.Add(Restrictions.Between(propertyName, lo, hi));
        }


        public static ICriteria Ge(this ICriteria criteria, string propertyName, object value) {
            return criteria.Add(Restrictions.Ge(propertyName, value));
        }


        public static ICriteria Gt(this ICriteria criteria, string propertyName, object value) {
            return criteria.Add(Restrictions.Gt(propertyName, value));
        }


        public static ICriteria Le(this ICriteria criteria, string propertyName, object value) {
            return criteria.Add(Restrictions.Le(propertyName, value));
        }


        public static ICriteria Lt(this ICriteria criteria, string propertyName, object value) {
            return criteria.Add(Restrictions.Lt(propertyName, value));
        }


        public static ICriteria In(this ICriteria criteria, string propertyName, params object[] values) {
            return criteria.Add(Restrictions.In(propertyName, values));
        }


        public static ICriteria In(this ICriteria criteria, string propertyName, ICollection values) {
            return criteria.Add(Restrictions.In(propertyName, values));
        }


        public static ICriteria NotIn(this ICriteria criteria, string propertyName, params object[] values) {
            return criteria.Add(Restrictions.Not(Restrictions.In(propertyName, values)));
        }


        public static ICriteria NotIn(this ICriteria criteria, string propertyName, ICollection values) {
            return criteria.Add(Restrictions.Not(Restrictions.In(propertyName, values)));
        }


        public static ICriteria StartsWith(this ICriteria criteria, string propertyName, string value) {
            return criteria.Add(Restrictions.Like(propertyName, value, MatchMode.Start));
        }


        public static ICriteria EndsWith(this ICriteria criteria, string propertyName, string value) {
            return criteria.Add(Restrictions.Like(propertyName, value, MatchMode.End));
        }


        public static ICriteria Contains(this ICriteria criteria, string propertyName, string value) {
            return criteria.Add(Restrictions.Like(propertyName, value, MatchMode.Anywhere));
        }


        public static ICriteria NotEq(this ICriteria criteria, string propertyName, object value) {
            return criteria.Add(Restrictions.Not(Restrictions.Eq(propertyName, value)));
        }


        public static ICriteria Or(this ICriteria criteria, ICriterion cri1, ICriterion cri2) {
            return criteria.Add(Restrictions.Or(cri1, cri2));
        }


        public static ICriteria IsNull(this ICriteria criteria, string propertyName) {
            return criteria.Add(Restrictions.IsNull(propertyName));
        }


        public static ICriteria IsNotNull(this ICriteria criteria, string propertyName) {
            return criteria.Add(Restrictions.IsNotNull(propertyName));
        }


        public static ICriteria RowCount(this ICriteria criteria) {
            return criteria.SetProjection(
                Projections.RowCount()
            );
        }


        public static Order GetOrder(string sortClause) {
            bool isDesc = (sortClause.EndsWith(" desc", StringComparison.InvariantCultureIgnoreCase));
            int index = sortClause.IndexOf(' ');
            
            if (index <= 0)
                index = sortClause.Length;

            string propertyName = sortClause.Substring(0, index);
            return (isDesc ? Order.Desc(propertyName) : Order.Asc(propertyName));
        }


        public static ICriteria AddOrder(this ICriteria criteria, string sortClause) {
            if (!sortClause.IsEmpty()) 
                criteria.AddOrder(GetOrder(sortClause));
            return criteria;
        }


        public static ICriteria OuterJoin(this ICriteria criteria, string associationPath) {
            return criteria.OuterJoin(associationPath, CreateAliasName(associationPath));
        }


        public static ICriteria OuterJoin(this ICriteria criteria, string associationPath, string alias) {
            return criteria
                .CreateAlias(associationPath, alias, JoinType.LeftOuterJoin)
                .SetFetchMode(associationPath, FetchMode.Join);
        }


        public static ICriteria Join(this ICriteria criteria, string associationPath) {
            return criteria.Join(associationPath, CreateAliasName(associationPath));
        }


        public static ICriteria Join(this ICriteria criteria, string associationPath, string alias) {
            return criteria.CreateAlias(associationPath, alias, JoinType.InnerJoin);
        }
        


        private static string CreateAliasName(string associationPath) {
            int index = associationPath.LastIndexOf(".");
            return associationPath.Substring(index + 1).ToLower();
        }
    }
}
