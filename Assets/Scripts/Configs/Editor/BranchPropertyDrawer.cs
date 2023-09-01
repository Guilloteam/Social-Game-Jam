using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

// IngredientDrawerUIE
//[CustomPropertyDrawer(typeof(BranchResultConfig))]
/*public class BranchResultConfigDrawer: PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create property container element.
        var container = new VisualElement();

        // Create property fields.
        var condition_property = property.FindPropertyRelative("condition_type");
        var condition_type = new PropertyField(condition_property);

        Toggle condition_toggle = new Toggle("Condition");
        condition_toggle.value = condition_property.intValue > 0;

        container.Add(condition_toggle);

        GroupBox group_box = new GroupBox();
        group_box.Add(new PropertyField(property.FindPropertyRelative("condition_operator")));
        group_box.Add(new PropertyField(property.FindPropertyRelative("condition_value")));
        container.Add(new PropertyField(property.FindPropertyRelative("display_text")));
        if(condition_property.intValue > 0)
            container.Add(group_box);
        condition_toggle.RegisterValueChangedCallback(evt => {
            //group_box.visible = evt.newValue;
            if (!evt.newValue)
                container.Remove(group_box);
            else container.Insert(1, group_box);
            condition_property.intValue = evt.newValue ? 1 : 0;
        });

        return container;
    }
}*/