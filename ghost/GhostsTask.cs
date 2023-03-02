using System;
using System.Text;

namespace hashes;

public class GhostsTask : 
	IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>, 
	IMagic
{
    private byte[] contentByteArray = new byte[] { 49, 74, 27, 73, 20, 66, 69, 6, 65 };
    private Cat cat = new Cat("Mr.Mondego", "white cutie", new DateTime(2004, 10, 20));
    private Vector vector = new Vector(42, 322);
    private Segment segment = new Segment(new Vector(256, 512), new Vector(1024, 2048));
    private Robot robot = new Robot("Factorio Drone", 55);

    public void DoMagic()
	{
        contentByteArray[5] = contentByteArray[1];
        cat.Rename("Vulduk");
        vector.Add(new Vector(4096, 8192));
        segment.End.Add(new Vector(4096, 8192));
        Robot.BatteryCapacity -= 100;
    }

	Vector IFactory<Vector>.Create() => vector;
	Segment IFactory<Segment>.Create() => segment;
    Cat IFactory<Cat>.Create() => cat;
    Robot IFactory<Robot>.Create() => robot;
    Document IFactory<Document>.Create() => new Document(
        "Ulearn practices hard? Read article about URFU programming.", Encoding.UTF8, contentByteArray);
}