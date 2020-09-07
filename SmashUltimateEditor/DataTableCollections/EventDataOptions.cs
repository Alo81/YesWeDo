using SmashUltimateEditor.DataTables;
using SmashUltimateEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmashUltimateEditor.DataTableCollections
{
    public class EventDataOptions : DataOptions
    {
        public List<Event> dataList;

        public EventDataOptions()
        {
            dataList = new List<Event>();
        }
        public int GetCount()
        {
            return dataList.Count;
        }

        public Event GetEvent(Type type, string label)
        {
            return GetEventsOfType(type).Where(x => x.GetFieldValueFromName("label") == label).FirstOrDefault();
        }

        public List<Event> GetEventsOfType(Type type)
        {
            return dataList.Where(x => x.GetType() == type).ToList();
        }
        public List<string> GetLabelsOfType(Type type)
        {
            return dataList.Where(x => x.GetType() == type).Select(x => ((LabelEvent)x).label).OrderBy(x => x).ToList();
        }
        public List<string> GetLabelsOfType(string type)
        {
            try
            {
                return dataList.Where(x => x.GetTypeName() == type).Select(x => ((LabelEvent)x)?.label).OrderBy(x => x).ToList();
            }
            catch (Exception ex)
            {
                UiHelper.PopUpCallingClass(String.Format("No Event labels or error getting Event labels of type: {0}.\r\n", type));
                return new List<String>() { "" };
            }
        }

        public int GetEventIndex(Type type, string label)
        {
            return GetEvents().FindIndex(x => x == GetEvent(type, label));
        }
        public int GetEventIndex(Event inEvent)
        {
            return dataList.FindIndex(x => x == inEvent);
        }
        public Event GetEventAtIndex(int index)
        {
            return dataList?[index];
        }
        public List<Event> GetEvents()
        {
            return dataList;
        }

        public void AddEvent(Event inEvent)
        {
            dataList.Add(inEvent);
        }
        public void SetEvents(List<Event> inEvents)
        {
            dataList = inEvents;
        }

        public void ReplaceEvents(EventDataOptions replacement)
        {
            foreach (var replEvent in replacement.GetEvents())
            {
                dataList[GetEventIndex(replEvent.GetType(), replEvent.GetFieldValueFromName("label"))] = replEvent;
            }
        }
        public void ReplaceEventAtIndex(int index, Event newEvent)
        {
            dataList[index] = newEvent;
        }
        public Type GetContainerType()
        {
            return dataList[0].GetType();
        }


        public List<string> event_type
        {
            get { return dataList.Select(x => x.GetTypeName()).Distinct().OrderBy(x => x).ToList(); }
        }
    }
}
