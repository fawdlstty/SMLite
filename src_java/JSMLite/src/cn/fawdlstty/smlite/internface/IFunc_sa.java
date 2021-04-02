package cn.fawdlstty.smlite.internface;

public interface IFunc_sa<TState> {
    public TState call (TState state, Object[] args) throws Exception;
}
