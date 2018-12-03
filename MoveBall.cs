using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Timers;

public class MoveBall : Form
{
	private const double refresh_rate = 40.0;
	private const double time_converter = 1000.0;
	private  double dot_update_rate; // = 200.0;
	private const double delta = 1.0;

	private Label title = new Label();
	private Label x_coordinate = new Label();
	private Label y_coordinate = new Label();
	private Label direction = new Label();

	private const int rectangle_x = 100;
	private const int rectangle_y = 100;
	private const int rectangle_length = 1400;
	private const int rectangle_width = 550;

	private string dir;
	private int x_pos;
	private int y_pos;
	private string xstr;
	private string ystr;

	private Point topleft = new Point(100, 100);
	private Point topright = new Point(1500, 100);
	private Point bottomright = new Point(1500, 650);
	private Point bottomleft = new Point(100, 650);

	private int radius = 10;
	private int diameter = 20;
	private int x;
	private int y;

	Button start = new Button();
	Button reset = new Button();
	Button exit = new Button();
	Button pause = new Button();

	private static System.Timers.Timer refresh_clock = new System.Timers.Timer();
	private static System.Timers.Timer animation_clock = new System.Timers.Timer();

	private SolidBrush BallBrush = new SolidBrush(Color.Purple);

	Label animationrate_text = new Label();
	TextBox animationrate = new TextBox();

	public MoveBall()
	{
		Size = new Size(1600, 900);
		Text = "Assignment 3";

		dir = "right";
		x_pos = x + radius;
		y_pos = y + radius;
		xstr = x_pos.ToString();
		ystr = y_pos.ToString();

		start.Text = "start";
		start.Size = new Size(85, 25);
		start.Location = new Point(150, 710);
		start.Font = new Font("Arial", 10, FontStyle.Regular);
		reset.Text = "reset";
		reset.Size = new Size(85, 25);
		reset.Location = new Point(150, 750);
		pause.Text = "pause";
		pause.Size = new Size(85, 25);
		pause.Location = new Point(150, 790);
		exit.Text = "exit";
		exit.Size = new Size(85, 25);
		exit.Location = new Point(150, 830);

		animationrate_text.Text = "Refresh rate (Hz):";
		animationrate_text.Size = new Size(110, 25);
		animationrate_text.Location = new Point(350, 710);
		animationrate_text.ForeColor = Color.White;
		animationrate_text.BackColor = Color.Gray;
		animationrate.Size = new Size(100, 25);
		animationrate.Location = new Point(465, 710);

		title.Text = "Haowen -- assignment 3";
		title.Size = new Size(150, 25);
		title.Location = new Point(700, 10);
		title.BackColor = Color.White;
		x_coordinate.Text = "x coordinate: 100";
		x_coordinate.Size = new Size(150, 25);
		x_coordinate.Location = new Point(650, 710);
		x_coordinate.ForeColor = Color.White;
		x_coordinate.BackColor = Color.Gray;
		y_coordinate.Text = "y coordinate: 100";
		y_coordinate.Size = new Size(150, 25);
		y_coordinate.Location = new Point(650, 750);
		y_coordinate.ForeColor = Color.White;
		y_coordinate.BackColor = Color.Gray;
		direction.Text = "Direction: ";
		direction.Size = new Size(150, 25);
		direction.Location = new Point(650, 790);
		direction.ForeColor = Color.White;
		direction.BackColor = Color.Gray;

		x = topleft.X - radius;
		y = topleft.Y - radius;

		Controls.Add(start);
		Controls.Add(reset);
		Controls.Add(pause);
		Controls.Add(exit);
		Controls.Add(title);
		Controls.Add(x_coordinate);
		Controls.Add(y_coordinate);
		Controls.Add(direction);
		Controls.Add(animationrate_text);
		Controls.Add(animationrate);

		start.Click += new EventHandler(start_click);
		reset.Click += new EventHandler(reset_click);
		pause.Click += new EventHandler(pause_click);
		exit.Click += new EventHandler(exit_click);

		refresh_clock.Enabled = false;
		refresh_clock.Elapsed += new ElapsedEventHandler(update_graphics);

		animation_clock.Enabled = false;
		animation_clock.Elapsed += new ElapsedEventHandler(update_dot_position);

		DoubleBuffered = true;
	}
	
	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics board = e.Graphics;
		Pen blackpen = new Pen(Color.Black, 2);
		board.DrawLine(blackpen, 0, 50, 1600, 50);
		board.FillRectangle(Brushes.Black, 0, 0, 1600, 50);
		Pen redpen = new Pen(Color.Red, 1);
		board.DrawRectangle(redpen, rectangle_x, rectangle_y, rectangle_length, rectangle_width);
		board.DrawLine(blackpen, 0, 700, 1600, 700);
		board.FillRectangle(Brushes.Gray, 0, 700, 1600, 200);

		board.FillEllipse(BallBrush, x, y, diameter, diameter);

		base.OnPaint(e);
	}

	protected void start_refresh_clock(double refreshrate)
	{
		double elapsed_time_between_tics;
		if(refreshrate < 1.0)
 			refreshrate = 1.0;
		elapsed_time_between_tics = time_converter/refreshrate;
		refresh_clock.Interval = (int)System.Math.Round(elapsed_time_between_tics);
		refresh_clock.Enabled = true;
		System.Console.WriteLine("method start_refresh_clock has terminated, refresh_clock has started");
	}

	protected void start_animation_clock(double updaterate)
	{
		double elapsed_time_between_coordinate_changes;
		if(updaterate < 1.0)
			updaterate = 1.0;
		elapsed_time_between_coordinate_changes = time_converter/updaterate;
		animation_clock.Interval = (int)System.Math.Round(elapsed_time_between_coordinate_changes);
		animation_clock.Enabled = true;
		System.Console.WriteLine("method start_animation_clock has terminated, animation_clock has started.");
	}

	protected void update_graphics(System.Object sender, ElapsedEventArgs even)
	{
		Invalidate();
	}

	protected void update_dot_position(System.Object sender, ElapsedEventArgs e)
	{
		x_coordinate.Text = "x coordinate: ";
		y_coordinate.Text = "y coordinate: ";
		direction.Text = "Direction: ";
		Point test = new Point(x + radius, y + radius);
		if(test.Y == 100)
		{
			int tempx = x + (int)delta + radius;
			if(tempx < 1500)
			{
				x += (int)delta;
			}
			else
			{
				x = 1500 - radius;
				y = 101 - radius;
				dir = "down";
			}
		}
		else if(test.Y == 650)
		{
			int tempx = x + radius - (int)delta;
			if(tempx > 100)
				x -= (int)delta;
			else {x = 100 - radius; y = 649 - radius; dir = "up";}
		}
		else
		{
			if(test.X == 100)
			{
				int tempy = y + radius -(int)delta;
				if(tempy > 100)
					y -= (int)delta;
				else 
				{
					refresh_clock.Enabled = false;
					animation_clock.Enabled = false;
					y = 100 - radius;
					y_coordinate.Text = "y coordinate: ";
					BallBrush = new SolidBrush(Color.Gold);
					Invalidate();
				}
			}
			else if(test.X == 1500)
			{
				int tempy = y + radius + (int)delta;
				if(tempy < 650)
					y += (int)delta;
				else {y = 650 - radius; x = 1500 - radius; dir = "left";}
			}
			else {System.Console.WriteLine("the ball seems to be out of position."); Close();}
		}
		
		x_pos = x + radius;
		y_pos = y + radius;
		xstr = x_pos.ToString();
		ystr = y_pos.ToString();
		x_coordinate.Text += xstr;
		y_coordinate.Text += ystr;
		direction.Text += dir;
	}

	protected void start_click(Object sender, EventArgs events)
	{
		double anim_rate = Double.Parse(animationrate.Text);
		dot_update_rate = anim_rate;
		if(pause.Text == "resume")
			pause.Text = "pause";
		BallBrush.Color = Color.Purple;
		direction.Text = "Direction: right";
		start_refresh_clock(refresh_rate);
		start_animation_clock(dot_update_rate);
		x_coordinate.Text += xstr;
		y_coordinate.Text += ystr;
		direction.Text += dir;
		System.Console.WriteLine("you've clicked on the start button.");
	}

	protected void reset_click(Object sender, EventArgs events)
	{
		if(pause.Text == "resume")
			pause.Text = "pause";
		BallBrush.Color = Color.Purple;
		x = 100 - radius;
		y = 100 - radius;
		x_coordinate.Text = "x coordinate: 100";
		y_coordinate.Text = "y coordinate: 100";
		refresh_clock.Enabled = false;
		animation_clock.Enabled = false;
		System.Console.WriteLine("you've clicked on the reset button.");
		Invalidate();
	}

	protected void pause_click(Object sender, EventArgs events)
	{
		if(pause.Text == "pause")
		{
			refresh_clock.Enabled = false;
			animation_clock.Enabled = false;
			pause.Text = "resume";
		}
		else
		{
			refresh_clock.Enabled = true;
			animation_clock.Enabled = true;
			pause.Text = "pause";
		}
	}


	protected void exit_click(Object sender, EventArgs events)
	{
		System.Console.WriteLine("you've clicked on the exit button.");
		Close();
	}
}