package cn.fawdlstty.smlite;

public class Tuple2<A extends Comparable, B extends Comparable> implements Comparable {
    public final A first;
    public final B second;

    public Tuple2(A a, B b) {
        this.first = a;
        this.second = b;
    }

    @Override
    public int compareTo(Object o) {
        Tuple2<A,B> _o = (Tuple2<A,B>) o;
        if (this.first == _o.first && this.second == _o.second)
            return 0;
        return -1;
    }
}
