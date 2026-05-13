using System;
using System.Linq;
using System.Collections.Generic;

namespace Get_and_Set;

public class Order
{
    private int receiptNo;
    private double finalTotal;

    public int ReceiptNo
    {
        get { return receiptNo; }
        set { receiptNo = value; }
    }

    public double FinalTotal
    {
        get { return finalTotal; }
        set { finalTotal = value; }
    }

    public Order(int receiptNo, double total)
    {
        this.receiptNo = receiptNo;
        this.finalTotal = total;
    }
}

