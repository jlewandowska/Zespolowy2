using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace EventEntityNamespace
{
    public class EventEntityHelper
    {
        static private string createStringPos(int x, int y, int z)
        {
            return x.ToString() + ";" + y.ToString() + ";" + z.ToString();
        }

        // TODO get player pos
        static public string createPlayerPos()
        {
            return createStringPos(1,1,1);
        }

        // TODO get enemy pos
        static public string createEnemyPos()
        {
            return createStringPos(1, 1, 1);
        }
    }
    public class EventEntity
    {
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

        public EventEntity(string _type, string _id)
        {
            DateTime now = DateTime.Now;
            latency = now.ToString("yyyyMMddHHmmss");

            type = _type;

            id = _id;
            enemy_pos = EventEntityHelper.createEnemyPos();
            player_pos = EventEntityHelper.createPlayerPos();

            //TODO get room 
            room = "";
            level = GameFlowManager.roundCnt.ToString();

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

        private void saveToStymulationFile()
        {
            string fileName = "stymulacja.txt";

            while (IsFileLocked(fileName)) { }

            using (StreamWriter writetext = new StreamWriter(fileName))
            {
                string entity = "";

                void addToEntity(string val)
                {
                    entity += val;
                    entity += ",";
                }

                entity += "{";

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
                addToEntity(power);

                entity += "}";


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