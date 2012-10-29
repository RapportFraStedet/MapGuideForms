// Copyright 2012, MapGuideForm user group, Frederikssund Kommune and Helsingør Kommune - att. Anette Poulsen and Erling Kristensen
// 
// This file is part of "RapportFraStedet". 
// "RapportFraStedet" is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or any later version.
// "RapportFraStedet" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License along with "RapportFraStedet". If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RapportFraStedet.Models;

namespace RapportFraStedet.Models
{
    public class RepositoryData
    {/*
        public CreateModel Get(Form form)
        {
            List<FieldModel> dataFields = new List<FieldModel>();
            foreach(Field field in form.Fields.OrderBy(a=>a.FieldOrder))
            {
                List<SelectionModel> selections = new List<SelectionModel>();
                foreach (Selection selection in field.Selections)
                {
                    selections.Add(new SelectionModel() { 
                    Id = selection.SelectionId,
                    Name = selection.Name,
                    Selected = false,
                    Selection = selection
                    });
                }
                dataFields.Add(new FieldModel(field)
                {
                    Selections = new SelectList(selections, "Id", "Name")
                }); 
            }
            CreateModel model = new CreateModel()
            {
                FormId = form.FormId,
                Name = form.Name,
                ToolType = form.GeometryTypeId,
                DataFields = dataFields,
                GroupId = -1
            };
            return model;
        }
        public CreateModel GetGroup(Group group, Form form)
        {
            List<FieldModel> dataFields = new List<FieldModel>();
            foreach (Field field in form.Fields.OrderBy(a => a.FieldOrder))
            {
                List<SelectionModel> selections = new List<SelectionModel>();
                foreach (Selection selection in field.Selections)
                {
                    selections.Add(new SelectionModel()
                    {
                        Id = selection.SelectionId,
                        Name = selection.Name,
                        Selected = false,
                        Selection = selection
                    });
                }
                dataFields.Add(new FieldModel(field)
                {
                    Selections = new SelectList(selections, "Id", "Name")
                });
            }
            CreateModel model = new CreateModel()
            {
                GroupName = group.Name,
                FormId = form.FormId,
                Forms = new SelectList(group.Forms,"FormId","Name",form.FormId),
                Name = form.Name,
                GroupId = group.GroupId,
                DataFields = dataFields,
                ToolType = form.GeometryTypeId
            };
            return model;
        }*/
        /*
        public DataFormModel GetGroup(int id, int formId)
        {
            RepositoryGroups repositoryGroups = new RepositoryGroups();
            Group group = repositoryGroups.Get(id);
            RepositoryForms repositoryForms = new RepositoryForms();
            group.DefaultFormId = formId;
            Form form = repositoryForms.Get(group.DefaultFormId.Value);
            List<DataFieldModel> dataFields = new List<DataFieldModel>();
            foreach (Field field in form.Fields)
            {
                List<DataSelectionModel> selections = new List<DataSelectionModel>();
                foreach (Selection selection in field.Selections)
                {
                    selections.Add(new DataSelectionModel()
                    {
                        Id = selection.SelectionId,
                        Name = selection.Name,
                        Selected = false,
                        Selection = selection
                    });
                }
                dataFields.Add(new DataFieldModel()
                {
                    Field = field,
                    Selections = new SelectList(selections, "Id", "Name")
                });
            }
            DataFormModel model = new DataFormModel()
            {
                Form = form,
                GroupId = id,
                Group = group,
                DataFields = dataFields,
                SelectedForm = form.FormId
            };
            return model;
        }*/
    }
}