using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace DocumentSetContentTypeDemo.LoanRequestorImageWebPart
{
    [ToolboxItemAttribute(false)]
    public class LoanRequestorImageWebPart : WebPart
    {
        const string NO_IMAGE_AVAILABLE = "/_layouts/Images/DocumentSetContentTypeDemo/NoImageUploaded.jpg";

        protected override void CreateChildControls()
        {
            if (this.Page.Request["ID"] != null)
            {
                int docSetId = Convert.ToInt32(this.Page.Request["ID"].ToString());
                SPList currentList = SPContext.Current.Web.Lists[new Guid(this.Page.Request["List"].ToString())];
                SPFolder folder = currentList.GetItemById(docSetId).Folder;
                string docSetRootFolder = this.Page.Request["RootFolder"].ToString();

                if (currentList != null)
                {
                    SPQuery qry = new SPQuery();
                    qry.Query = @"<Where>
                                  <Eq>
                                     <FieldRef Name='ContentType' />
                                     <Value Type='Computed'>Image</Value>
                                  </Eq>
                               </Where>";
                    qry.ViewAttributes = "<View Scope=\"RecursiveAll\"> ";
                    qry.Folder = folder;
                    SPListItemCollection listItems = currentList.GetItems(qry);

                    if (listItems.Count > 0)
                    {
                        SPListItem imageItem = listItems[0];
                        string name = imageItem["Name"].ToString();
                        string imageUrl = docSetRootFolder +"/" + name;
                        this.Controls.Add(new LiteralControl(string.Format("<img src='{0}' alt='{1}' height='150' width='140' style='padding:8px'/>", imageUrl, name)));
                    }
                    else
                    {
                        this.Controls.Add(new LiteralControl(string.Format("<img src='{0}' height='150' width='140' style='padding:8px'/>", NO_IMAGE_AVAILABLE)));
                    }
                }
            }
            else
            {
                this.Controls.Add(new LiteralControl(string.Format("<img src='{0}' height='150' width='140' style='padding:8px'/>", NO_IMAGE_AVAILABLE)));
            }
        }
    }
}
