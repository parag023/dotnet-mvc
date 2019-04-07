using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web.classes
{
    public class contacts
    {
        private int cid;
        private string contactnos;

        public int Cid
        {
            get { return this.cid; }
            set { this.cid = value; }
        }

        public string ContactNos
        {
            get { return this.contactnos; }
            set { this.contactnos = value; }
        }
         
        //public void ContactNumber(int cid,string contactnos)
        //{
        // this.cid=cid;
        // this.contactnos=contactnos;

        //}
        public contacts(int cid, string contactnos)
        {
            this.cid = cid;
            this.contactnos = contactnos;
        }

        public contacts()
        { }

    }
}