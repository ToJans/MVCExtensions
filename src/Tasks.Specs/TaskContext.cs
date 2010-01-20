using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Services.Impl;
using Tasks.Core.Model;
using MvcExtensions.Services;
using Be.Corebvba.Aubergine.Model;

namespace Tasks.Specs
{
    public class TaskContext
    {
        const string DONE = "Done";

        IRepository rTask = new FakeRepository();

        public TaskContext()
        {
        }

        [DSL("the following tasks exist")]
        [DSL("I add the following task")]
        public void The_Following_tasks(string[] name, string[] description, string[] status)
        {
            for (var i = 0; i < name.Length; i++)
            {
                rTask.SaveOrUpdate(new Task() 
                {
                    Name=name[i], Description= description[i],Done = DONE.Equals(status[i])
                });
            }
        }

        [DSL("the tasklist should contain the following tasks")]
        public bool ContainsTasks(string[] name, string[] description, string[] status)
        {
            var arr = rTask.Find<Task>().ToArray();
            if (arr.Length != name.Length) return false;
            for (int i = 0; i < name.Length; i++)
            {
                if ((arr[i].Name != name[i]) || 
                    (arr[i].Description != description[i]) ||
                    (arr[i].Done != DONE.Equals(status[i])))
                    return false;
            }
            return true;
        }

        [DSL("I switch the status of (?<task>.+)")]
        void SwitchStatus(Task task)
        {
            task.Done ^= true;
            rTask.SaveOrUpdate(task);
        }

        [DSL("I delete (?<task>.+)")]
        void Delete(Task task)
        {
            rTask.Delete(task);
        }

        [DSL("the task \"(?<name>.+)\"")]
        Task GetTask(string name)
        {
            return rTask.Find<Task>().Where(t => t.Name == name).FirstOrDefault();
        }



    }
}
