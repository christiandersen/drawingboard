using System;

namespace DrawingBoard2
{
    //Delegates
    /// <summary>
    /// Object Selected Event is and event that is occured when any shape on the board is selected
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="e">EventArgs of event</param>
    public delegate void ObjectSelectedEvent(object sender, PropertyEventArgs e);

    /// <summary>
    /// Represents whether an angle is specified in degrees or radians.
    /// </summary>
    public enum AngleMeasurement
    {
        /// <summary>
        /// Angles are represented in degrees
        /// </summary>
        Degrees,
        /// <summary>
        /// Angles are represented in radians
        /// </summary>
        Radians
    }
    /// <summary>
    /// Enumeration that represents the action 
    /// When resizing is done to a group of shape objects
    /// </summary>
    public enum OnGroupResize
    {
        /// <summary>
        /// When a group is resized , group objects move
        /// </summary>
        Move,
        /// <summary>
        /// When a group is resized , group objects are resized
        /// </summary>
        Resize,
        /// <summary>
        /// When a group is resized , group objects keep their size and location
        /// </summary>
        Nothing
    }
    /// <summary>
    /// Enumeration that represents direction
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// West
        /// </summary>
        West,
        /// <summary>
        /// East
        /// </summary>
        East,
        /// <summary>
        /// North
        /// </summary>
        North,
        /// <summary>
        /// South
        /// </summary>
        South,
        /// <summary>
        /// South East
        /// </summary>
        SouthEast,
        /// <summary>
        /// South West
        /// </summary>
        SouthWest,
        /// <summary>
        /// NorthEast
        /// </summary>
        NorthEast,
        /// <summary>
        /// NorthWest
        /// </summary>
        NorthWest
    }
    /// <summary>
    /// Group Display options
    /// </summary>
    public enum GroupDisplay
    {
        /// <summary>
        /// Default Display.
        /// 
        /// </summary>
        Default,
        /// <summary>
        /// Interset
        /// <see cref="System.Drawing.Region">Region.Intersect() will be invoked</see>
        /// </summary>
        Intersect,
        /// <summary>
        /// Xor
        /// <see cref="System.Drawing.Region">Region.Xor() will be invoked</see>
        /// </summary>
        Xor,
        /// <summary>
        /// Exclude
        ///  <see cref="System.Drawing.Region">Region.Exclude() will be invoked</see>
        /// </summary>
        Exclude
    };
    /// <summary>
    /// Operators for handlers(handling).Indicates current operation of the user
    /// </summary>
    public enum HandlerOperator
    {
        /// <summary>
        /// Default operator
        /// </summary>
        Default,
        /// <summary>
        /// Redimensioning West side
        /// </summary>
        RedimWest,
        /// <summary>
        /// Redimensioning East Side
        /// </summary>
        RedimEast,
        /// <summary>
        /// Redimensioning North Side
        /// </summary>
        RedimNorth,
        /// <summary>
        /// Redimensioning South Side
        /// </summary>
        RedimSouth,
        /// <summary>
        /// Redimensioning South-East Side
        /// </summary>
        RedimSouthEast,
        /// <summary>
        /// Redimensioning South-West Side
        /// </summary>
        RedimSouthWest,
        /// <summary>
        /// Redimensioning North-East Side
        /// </summary>
        RedimNorthEast,
        /// <summary>
        /// Redimensioning North-West Side
        /// </summary>
        RedimNorthWest,
        /// <summary>
        /// Zooming
        /// </summary>
        Zoom,
        /// <summary>
        /// Rotating shape
        /// </summary>
        Rotation,
        /// <summary>
        /// Adding new point to a point set
        /// </summary>
        NewPoint,
        /// <summary>
        /// Polygon operation
        /// </summary>
        Polygon,
        /// <summary>
        /// Not set
        /// </summary>
        None
    };
    /// <summary>
    /// Type of operation 
    /// </summary>
    public enum BufferOperation
    {
        /// <summary>
        /// Inserting shape 
        /// </summary>
        Insert,
        /// <summary>
        /// Updating shape
        /// </summary>
        Update,
        /// <summary>
        /// Deleting shape
        /// </summary>
        Delete
    };

    /// <summary>
    /// Used in DrawingBoard control to store status of drawing board
    /// </summary>
    public enum DrawingBoardStatus
    {
        /// <summary>
        /// Current Status is Redimension
        /// </summary>
        Redimension,
        /// <summary>
        /// Current status is drawing rectangle ( here , rectangle is used for any shape)
        /// </summary>
        DrawRectangle,
        /// <summary>
        /// Current status is selecting rectangle
        /// </summary>
        SelectRectangle,
        /// <summary>
        /// No status is set
        /// </summary>
        None,
    };
    /// <summary>
    /// Type of the Triangle shape
    /// </summary>
    public enum TriangleType
    {
        /// <summary>
        /// Right Triangle ( one 90 degree corner)
        /// </summary>
        Right,
        /// <summary>
        /// Equilateral Triangle ( 60 degree corners)
        /// </summary>
        Equilateral
    };
    /// <summary>
    /// Binding type that is used in Chemical molecules 
    /// </summary>
    public enum BindingType
    {
        /// <summary>
        /// Single binding ( 1 bound between elements)
        /// </summary>
        Single = 1,
        /// <summary>
        /// Double binding ( 2 bounds between elements)
        /// </summary>
        Double = 2,
        /// <summary>
        /// Triple binding ( 3 bounds between elements)
        /// </summary>
        Triple = 3
    }
    /// <summary>
    /// Formula types
    /// </summary>
    public enum FormulaType
    {
        /// <summary>
        /// Chemistry Formula
        /// </summary>
        Chemistry,
        /// <summary>
        /// Mathmatics Formula
        /// </summary>
        Math
    }
    /// <summary>
    /// All public properties of the shapes 
    /// </summary>
    public enum ShapeProperty
    {
        /// <summary>
        /// Auto Generate Name of Corners
        /// </summary>
        EnableCornerNameGeneration,
        /// <summary>
        /// Rotation angle 
        /// </summary>
        Rotation,
        /// <summary>
        /// Show/Hide Border
        /// </summary>
        ShowBorder,
        /// <summary>
        /// Dash Style of the shape
        /// </summary>
        DashStyle,
        /// <summary>
        /// Start angle of arc/pie shapes
        /// </summary>
        StartAngle,
        /// <summary>
        /// Length of the angle of arc/pie shapes
        /// </summary>
        AngleLength,
        /// <summary>
        /// Width of arcs of Rounded Rectangle
        /// </summary>
        ArcsWidth,
        /// <summary>
        /// Text Alignent of SimpleTextBox
        /// </summary>
        TextAlignment,
        /// <summary>
        /// Start cap style of shapes
        /// </summary>
        StartCap,
        /// <summary>
        /// End cap style of shapes
        /// </summary>
        EndCap,
        /// <summary>
        /// Color of the text on the shape
        /// </summary>
        TextColor,
        /// <summary>
        /// Text of the shape(this property isnt avaliable for SimpleTextBox and RichTextBox shapes)
        /// </summary>
        Text,
        /// <summary>
        /// Corners of polygon are fixed(immovable) or not
        /// </summary>
        FixedCorners,
        /// <summary>
        /// Row count of Table shape
        /// </summary>
        RowCount,
        /// <summary>
        /// Column count of Table shape
        /// </summary>
        ColumnCount,
        /// <summary>
        /// Vertical unit count of cartesian plane
        /// </summary>
        VerticalUnitCount,
        /// <summary>
        /// Horizontal unit count of cartesian plane
        /// </summary>
        HorizontalUnitCount,
        /// <summary>
        /// Enable/Disable gradient brush
        /// </summary>
        UseGradientColor,
        /// <summary>
        /// Gradient Color(Second color) 
        /// </summary>
        GradientColor,
        /// <summary>
        /// Alpha value of gradient color
        /// </summary>
        GradientAlpha,
        /// <summary>
        /// Alpha value of main color
        /// </summary>
        ColorAlpha
    }
    /// <summary>
    /// Drawing Options
    /// </summary>
    public enum DrawingOption
    {
        /// <summary>
        /// Drawing Mouse select 
        /// </summary>
        Select,
        /// <summary>
        /// Drawing Ellipse 
        /// </summary>
        Ellipse,
        /// <summary>
        /// Drawing Pen
        /// </summary>
        Pen,
        /// <summary>
        /// Drawing line 
        /// </summary>
        Line,
        /// <summary>
        /// Drawing polygon
        /// </summary>
        Polygon,
        /// <summary>
        /// Drawing ImageBox 
        /// </summary>
        ImageBox,
        /// <summary>
        /// Drawing Arc 
        /// </summary>
        Arc,
        /// <summary>
        /// Drawing simple textbox
        /// </summary>
        SimpleTextBox,
        /// <summary>
        /// Drawing Rich textbox
        /// </summary>
        RichTextBox,
        /// <summary>
        /// Drawing Rectangle
        /// </summary>
        Rectangle,
        /// <summary>
        /// Drawing Rounded Rectangle
        /// </summary>
        RoundedRectangle,
        /// <summary>
        /// Drawing Triangle
        /// </summary>
        Triangle,
        /// <summary>
        /// Drawing Pentagon
        /// </summary>
        Pentagon,
        /// <summary>
        /// Drawing Hexagon
        /// </summary>
        Hexagon,
        /// <summary>
        /// Drawing Regular Polygon
        /// </summary>
        RegularPolygon,
        /// <summary>
        /// Drawing Star 
        /// </summary>
        Star,
        /// <summary>
        /// Drawing Trapezoid
        /// </summary>
        Trapezoid,
        /// <summary>
        /// Drawing Cube
        /// </summary>
        Cube,
        /// <summary>
        /// Drawing Cartesian Plane
        /// </summary>
        CartesianPlane,
        /// <summary>
        /// Drawing Table
        /// </summary>
        Table,
        /// <summary>
        /// Drawing Pie
        /// </summary>
        Pie
    };
}