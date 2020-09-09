using SmashUltimateEditor.DataTables;
using SmashUltimateEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmashUltimateEditor.DataTableCollections
{
    public class EventDataOptions : BaseDataOptions, IDataOptions
    {
        public List<Event> _dataList;
        public List<IDataTbl> dataList { get { return _dataList.OfType<IDataTbl>().ToList(); } }

        public EventDataOptions()
        {
            _dataList = new List<Event>();
        }
        public int GetCount()
        {
            return _dataList.Count;
        }

        public Event GetEvent(Type type, string label)
        {
            return GetEventsOfType(type).Where(x => x.GetFieldValueFromName("label") == label).FirstOrDefault();
        }

        public List<Event> GetEventsOfType(Type type)
        {
            return _dataList.Where(x => x.GetType() == type).ToList();
        }
        public List<string> GetLabelsOfType(Type type)
        {
            return _dataList.Where(x => x.GetType() == type).Select(x => ((LabelEvent)x).label).OrderBy(x => x).ToList();
        }
        public List<string> GetLabelsOfType(string type)
        {
            try
            {
                return _dataList.Where(x => x.GetTypeName() == type).Select(x => ((LabelEvent)x)?.label).OrderBy(x => x).ToList();
            }
            catch (Exception ex)
            {
                //UiHelper.PopUpCallingClass(String.Format("No Event labels or error getting Event labels of type: {0}.\r\n", type));
                return new List<String>() { "" };
            }
        }

        public int GetEventIndex(Type type, string label)
        {
            return GetEvents().FindIndex(x => x == GetEvent(type, label));
        }
        public int GetEventIndex(Event inEvent)
        {
            return _dataList.FindIndex(x => x == inEvent);
        }
        public Event GetEventAtIndex(int index)
        {
            return _dataList?[index];
        }
        public List<Event> GetEvents()
        {
            return _dataList;
        }

        public void AddEvent(Event inEvent)
        {
            _dataList.Add(inEvent);
        }
        public void AddUniqueEvents(List<Event> events)
        {
            foreach(Event inEvent in events)
            {
                if (!_dataList.Contains(inEvent))
                {
                    _dataList.Add(inEvent);
                }
            }
        }
        public void AddUniqueEvents(List<ItemEvent> events)
        {
            var items = _dataList.OfType<ItemEvent>();
            foreach (ItemEvent inEvent in events)
            {
                if (!(items.Any(x =>  x.label == inEvent.label)))
                {
                    _dataList.Add(inEvent);
                }
            }
        }
        public void SetEvents(List<Event> inEvents)
        {
            _dataList = inEvents;
        }

        public void ReplaceEvents(EventDataOptions replacement)
        {
            foreach (var replEvent in replacement.GetEvents())
            {
                _dataList[GetEventIndex(replEvent.GetType(), replEvent.GetFieldValueFromName("label"))] = replEvent;
            }
        }
        public void ReplaceEventAtIndex(int index, Event newEvent)
        {
            _dataList[index] = newEvent;
        }

        public List<string> event_type
        {
            get { return _dataList.Select(x => x.GetTypeName()).Distinct().OrderBy(x => x).ToList(); }
        }
    }
}
