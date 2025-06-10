var studSer = new StudentSerializer();

studSer.AddStudent("SCA01", "Alice", "WA", 3.6);
studSer.AddStudent("SCA02", "Bob", "CA", 3.8);
studSer.AddStudent("CAD01", "Charlie", "TX", 3.2);
studSer.AddStudent("CAD02", "Denise", "NC", 3.4);

studSer.jsonSerialize(0);
studSer.jsonDeserialize("SCA01.json");
studSer.xmlSerialize(0);
studSer.xmlDeserialize("SCA01.xml");