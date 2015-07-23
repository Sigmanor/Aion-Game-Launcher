using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
namespace System.Globalization
{
    class ChangeLanguage
    {
        /// <summary>
        /// Доступ к функции для проведения локализации.
        /// </summary>
        public static ChangeLanguage Instance
        {
            get
            {
                return new ChangeLanguage();
            }
        }
        /// <summary>
        /// Проведение процесса локализации заданной формы.
        /// </summary>
        /// <param name="editForm">Изменяемая форма</param>
        /// <param name="cultureInfo">Новая культура</param>
        public void localizeForm(Form editForm, CultureInfo cultureInfo)
        {
            Thread.CurrentThread.CurrentUICulture = cultureInfo;// установка новой культуры для текущего процесса 

            Type editFormType = editForm.GetType();
            ResourceManager res = new ResourceManager(editFormType);

            ToolTip toolTip = GetToolTip(editForm);//Подразумевается, что на одной форме используется ТОЛЬКО ОДИН компонент ToolTip
            if (toolTip != null) //Установка локализации всплывающего текста, сделанного с помощью ToolTips.
            {
                SetToolTips(toolTip, (Control)editForm, res, cultureInfo);
            }
            SetListControls(editForm, res, cultureInfo);

            //зададим список свойств объектов, которые будем извлекать из файла ресурсов
            string[] properties = { "Text", "Location", "Size", "Image" };

            foreach (string propertyName in properties)
            {
                //выбор всех свойств класса формы, извлечение из файла ресурсов значения, и их установка
                foreach (FieldInfo fieldInfo in editFormType.GetFields(BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance))
                {
                    PropertyInfo propertyInfo = fieldInfo.FieldType.GetProperty(propertyName);
                    if (propertyInfo == null) //есть ли данное свойство
                        continue;

                    object objProperty = res.GetObject(fieldInfo.Name + '.' + propertyInfo.Name, cultureInfo);
                    if (objProperty == null)
                        continue;//есть ли в ресурсах для данного объекта данное свойство

                    object field = fieldInfo.GetValue(editForm);
                    if (field != null) //отражение объекта с формы
                        propertyInfo.SetValue(field, objProperty, null); //установили свойство из properties
                }
                //код для установки свойств самих форм
                PropertyInfo propertyInfo1 = editFormType.GetProperty(propertyName);
                if (propertyInfo1 == null)
                    continue;
                object objProperty1 = res.GetObject("$this." + propertyInfo1.Name, cultureInfo);
                if (objProperty1 == null)
                    continue;
                propertyInfo1.SetValue(editForm, objProperty1, null);
            }
        }
        /// <summary>
        /// Установка локализации элементов типа ListControl {== Combobox, Lisbox}.
        /// </summary>
        /// <param name="ctrl">Control</param>
        /// <param name="res">Файл ресурсов</param>
        /// <param name="cultureInfo">Новая культура</param>
        private void SetListControls(Control ctrl, ResourceManager res, CultureInfo cultureInfo)
        {
            //System.Collections.IList lst=null;
            if (ctrl is ComboBox)
            {
                if (((ComboBox)ctrl).Items.Count > 0)
                {
                    int selectedIndex = ((ListControl)ctrl).SelectedIndex;

                    string objProperty = ctrl.Name + ".Items";
                    object obj = res.GetString(objProperty, cultureInfo);
                    // VS2002 generates item resource name with additional ".Items" string
                    /* if (obj == null) {
                         objProperty += ".Items";
                         obj = resources.GetString(objProperty, m_cultureInfo);
                     }*/
                    if (obj != null)
                    {
                        int itemsNumber = ((ComboBox)ctrl).Items.Count;
                        ((ComboBox)ctrl).Items.Clear();
                        //Debug.Assert(obj != null);
                        ((ComboBox)ctrl).Items.Add(obj);
                        for (int i = 1; i < itemsNumber; i++)
                            ((ComboBox)ctrl).Items.Add(res.GetString(objProperty + i.ToString(), cultureInfo));
                    }
                    ((ComboBox)ctrl).SelectedIndex = selectedIndex;
                }
            }
            else
                if (ctrl is ListBox)
                {
                    if (((ListBox)ctrl).Items.Count > 0)
                    {
                        int selectedIndex = ((ListControl)ctrl).SelectedIndex;

                        string objProperty = ctrl.Name + ".Items";
                        object obj = res.GetString(objProperty, cultureInfo);
                        // VS2002 generates item resource name with additional ".Items" string
                        /* if (obj == null) {
                             objProperty += ".Items";
                             obj = resources.GetString(objProperty, m_cultureInfo);
                         }*/
                        if (obj != null)
                        {
                            int itemsNumber = ((ListBox)ctrl).Items.Count;
                            ((ListBox)ctrl).Items.Clear();
                            //Debug.Assert(obj != null);
                            ((ListBox)ctrl).Items.Add(obj);
                            for (int i = 1; i < itemsNumber; i++)
                                ((ListBox)ctrl).Items.Add(res.GetString(objProperty + i.ToString(), cultureInfo));
                        }
                        ((ListBox)ctrl).SelectedIndex = selectedIndex;
                    }
                }
                else
                {
                    foreach (Control control in ctrl.Controls)
                        SetListControls(control, res, cultureInfo);
                }
        }
        /// <summary>
        /// Устанавливаем на Control toolTips. Далее рекурсивно смотрятся все Control, входящие в заданный Control
        /// </summary>
        /// <param name="toolTip">Найденный на форме ToolTip</param>
        /// <param name="ctrl">Control</param>
        /// <param name="res">Файл ресурсов</param>
        /// <param name="cultureInfo">Новая культура</param>
        private void SetToolTips(ToolTip toolTip, Control ctrl, ResourceManager res, CultureInfo cultureInfo)
        {
            object objProperty = res.GetObject(ctrl.Name + ".ToolTip", cultureInfo);
            if (objProperty != null)//есть ли в ресурсах для данного объекта данное свойство
            {
                toolTip.SetToolTip(ctrl, (string)objProperty);
            }
            foreach (Control control in ctrl.Controls)
                SetToolTips(toolTip, control, res, cultureInfo);
        }
        /// <summary>
        /// Поиск на заданной форме элемента ToolTip.
        /// Подразумевается, что на форме используется ТОЛЬКО ОДИН компонент ToolTip
        /// </summary>
        /// <param name="control">
        /// Форма, на которой требуется найти ToolTip
        /// </param>
        /// <returns>
        ///     <c>ToolTip</c> of the control or <c>null</c> if not defined.
        /// </returns>
        /// 
        private ToolTip GetToolTip(System.Windows.Forms.Control control)
        {
            //Debug.Assert(control is System.Windows.Forms.Form || control is System.Windows.Forms.UserControl);
            FieldInfo[] fieldInfo = control.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                object obj = fieldInfo[i].GetValue(control);
                if (obj is System.Windows.Forms.ToolTip)
                    return (ToolTip)obj;
            }
            return null;
        }
    }
}
