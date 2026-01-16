using System;
using System.Collections.Generic;
using UnityEngine;

namespace My.Save
{
    [Serializable]
    public class TestCharacters
    {
        [SerializeField]
        private List<TestCharacter> characters;
        
        public List<TestCharacter> CharacterList => characters;

        public TestCharacters()
        {
            characters = new List<TestCharacter>()
            {
                new TestCharacter(1, "test1"),
            };
        }
        
        public TestCharacters(List<TestCharacter> characters)
        {
            this.characters = characters;
        }

        public string GetLogString()
        {
            string listStr = string.Join(", ", characters.ConvertAll(c => c.GetLogString()));
            return $"Characters : {listStr}";
        }
    }

    [Serializable]
    public class TestCharacter
    {
        [SerializeField]
        private int id;
        
        [SerializeField]
        private string name;

        public int Id => id;
        public string Name => name;

        public TestCharacter(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public string GetLogString()
        {
            return $"(Id : {id}, Name : {name})";
        }
    }
    
    [Serializable]
    public class TestEnemys
    {
        [SerializeField]
        private List<TestEnemy> enemys;
        
        public List<TestEnemy> EnemyList => enemys;

        public TestEnemys()
        {
            enemys = new List<TestEnemy>()
            {
                new TestEnemy(1, "testEnemy1", "Normal"),
            };
        }
        
        public TestEnemys(List<TestEnemy> enemys)
        {
            this.enemys = enemys;
        }

        public string GetLogString()
        {
            string listStr = string.Join(", ", enemys.ConvertAll(e => e.GetLogString()));
            return $"TestEnemys {listStr}";
        }
    }
    
    [Serializable]
    public class TestEnemy
    {
        [SerializeField]
        private int id;
        
        [SerializeField]
        private string name;
        
        [SerializeField]
        private string type;

        public int Id => id;
        public string Name => name;
        public string Type => type;

        public TestEnemy(int id, string name, string type)
        {
            this.id = id;
            this.name = name;
            this.type = type;
        }

        public string GetLogString()
        {
            return $"(Id : {id}, Name : {name}, Type : {type})";
        }
    }
}