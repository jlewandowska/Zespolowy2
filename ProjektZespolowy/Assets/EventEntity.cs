using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace EventEntityNamespace
{
    public class EventEntityHelper
    {
        static public string createStringPos(int x, int y, int z)
        {
            return x.ToString() + ";" + y.ToString() + ";" + z.ToString();
        }

        static public string createPlayerPos(Vector3 pos)
        {
            int z = Convert.ToInt32(pos.y);
            int x = Convert.ToInt32(pos.x);
            int y = Convert.ToInt32(pos.z);
            return createStringPos(x, y, z);
        }

        // TODO get enemy pos
        static public string createEnemyPos()
        {
            if (GameObject.FindGameObjectsWithTag("EnemyHover").Length > 0)
            {
                var enemy = GameObject.FindGameObjectsWithTag("EnemyHover")[0];
                var pos = enemy.transform.position;

                int z = Convert.ToInt32(pos.y);
                int x = Convert.ToInt32(pos.x);
                int y = Convert.ToInt32(pos.z);
                return createStringPos(x, y, z);
            }
                

            return createStringPos(-1, -1, -1);
        }
    }
    public class EventEntity : ScriptableObject
    {
        GameFlowManager m_GameFlowManager;
        string latency;
        string type;
        string id;
        string action;
        string sender;
        string device;
        string source;
        string enemy_pos;
        string player_pos;
        string level;
        string room;
        string direction;
        string question;
        string answer;
        string other;
        string channel;
        string band;
        string power;

        public void Init(string _type, string _id = "None")
        {
            m_GameFlowManager = FindObjectOfType<GameFlowManager>();

            DateTime now = DateTime.Now;
            latency = now.ToString("yyyyMMddHHmmssfff");

            type = _type;

            id = _id;
            enemy_pos = EventEntityHelper.createEnemyPos();
            if (m_GameFlowManager)
            {
                player_pos = EventEntityHelper.createPlayerPos(m_GameFlowManager.getPlayerPos());
            }
            else
                player_pos = EventEntityHelper.createStringPos(-1, -1, -1);

            room = m_GameFlowManager.getRoomNumber().ToString();
            level = m_GameFlowManager.getRoundNumber().ToString();

            sender = "application";
            device = "kognit";
            source = "Plik.txt";
            action = "None";
            direction = "None";
            question = "None";
            answer = "None";
            other = "None";
            channel = "None";
            band = "None";
            power = "None";
            saveToStymulationFile();
        }
        public static EventEntity CreateInstance(string _type, string _id = "None")
        {
            var data = ScriptableObject.CreateInstance<EventEntity>();
            data.Init(_type, _id);
            return data;
        }

        public EventEntity(string _type, string _id = "None")
        {
            m_GameFlowManager = FindObjectOfType<GameFlowManager>();

            DateTime now = DateTime.Now;
            latency = now.ToString("yyyyMMddHHmmssfff");

            type = _type;

            id = _id;
            enemy_pos = EventEntityHelper.createEnemyPos();
            player_pos = EventEntityHelper.createPlayerPos(m_GameFlowManager.getPlayerPos());

            //TODO get room  
            room = "room1";
            level = m_GameFlowManager.getRoundNumber().ToString();

            sender = "application";
            device = "kognit";
            source = "Plik.txt";
            action = "None";
            direction = "None";
            question = "None";
            answer = "None";
            other = "None";
            channel = "None";
            band = "None";
            power = "None";
            saveToStymulationFile();
        }

        public string toString()
        {
            string entity = "";

            void addToEntity(string val,bool comma = true)
            {
                entity += val;
                if(comma)
                    entity += ",";
            }

            addToEntity(latency);
            addToEntity(type);
            addToEntity(id);
            addToEntity(action);
            addToEntity(sender);
            addToEntity(device);
            addToEntity(source);
            addToEntity(enemy_pos);
            addToEntity(player_pos);
            addToEntity(level);
            addToEntity(room);
            addToEntity(direction);
            addToEntity(question);
            addToEntity(answer);
            addToEntity(other);
            addToEntity(band);
            addToEntity(channel);
            addToEntity(power,false);

            return entity;
        
    }

        private void saveToStymulationFile()
        {
            string fileName = "stymulacja.txt";

            while (IsFileLocked(fileName)) { }

            using (StreamWriter writetext = new StreamWriter(fileName))
            {
                string entity = toString();
                writetext.WriteLine(entity);
            }
        }

        private bool IsFileLocked(string fileName)
        {
            FileInfo file = new FileInfo(fileName);

            if (file.Exists == false)
                return false;

            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }

    }
}