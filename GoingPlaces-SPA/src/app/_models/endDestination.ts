export interface EndDestination{
    // public int Id { get; set; }
    // public string Goal { get; set; }
    // public int DistanceOfGoal { get; set; }
    // public User User { get; set; }

    id: number;
    goal?: string;
    distanceOfGoal?: string;
    userId: string;
    distance?: string;

}